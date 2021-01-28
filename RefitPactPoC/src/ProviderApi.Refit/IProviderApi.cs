using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProviderApi.Refit
{
    public interface IProviderApi
    {
        [Get("/weatherforecast")]
        Task<ApiResponse<IEnumerable<WeatherForecast>>> GetWeatherForecast();
    }
}
