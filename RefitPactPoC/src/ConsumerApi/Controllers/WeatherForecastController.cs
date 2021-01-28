using Microsoft.AspNetCore.Mvc;
using ProviderApi.Refit;
using Refit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IProviderApi _providerApi;

        public WeatherForecastController(IProviderApi providerApi)
        {
            _providerApi = providerApi;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            Task<ApiResponse<IEnumerable<ProviderApi.Refit.WeatherForecast>>> result = _providerApi.GetWeatherForecast();
            IEnumerable<ProviderApi.Refit.WeatherForecast> content = (await result).Content;
            return content.Select(x => new WeatherForecast { Date = x.Date, Summary = x.Summary, TemperatureC = x.TemperatureC });
        }
    }
}
