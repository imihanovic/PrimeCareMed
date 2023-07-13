using PrimeCareMed.Application.Models.WeatherForecast;

namespace PrimeCareMed.Application.Services;

public interface IWeatherForecastService
{
    public Task<IEnumerable<WeatherForecastResponseModel>> GetAsync();
}
