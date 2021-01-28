using Microsoft.Extensions.DependencyInjection;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using ProviderApi.Refit;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ConsumerApi.Tests
{
    public class ConsumerPactTests : IClassFixture<ConsumerPactClassFixture>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly IProviderApi _api;

        public ConsumerPactTests(ConsumerPactClassFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient(nameof(IProviderApi))
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(fixture.MockProviderServiceBaseUri))
                .AddTypedClient(RestService.For<IProviderApi>);

            var provider = serviceCollection.BuildServiceProvider();
            _api = provider.GetRequiredService<IProviderApi>();
        }

        [Fact]
        public async Task ItCanGeneratePactFile()
        {
            // Arrange
            _mockProviderService.Given("There is data")
                .UponReceiving("A GET request to retrieve the weather forecasts")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/weatherforecast"
                })
                .WillRespondWith(new ProviderServiceResponse {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    }
                });

            // Act

            // No need to test that 'GetWeatherForecast' works because it's already tested in the Provider
            var result = await _api.GetWeatherForecast();
        }
    }
}
