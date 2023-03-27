using BookIt.Application.Models.WeatherForecast;

namespace BookIt.Application.Services;

public interface IWeatherForecastService
{
    public Task<IEnumerable<WeatherForecastResponseModel>> GetAsync();
}
