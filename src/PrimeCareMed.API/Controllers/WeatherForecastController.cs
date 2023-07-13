using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeCareMed.Application.Models;
using PrimeCareMed.Application.Models.WeatherForecast;
using PrimeCareMed.Application.Services;

namespace PrimeCareMed.API.Controllers;

[Authorize]
public class WeatherForecastController : ApiController
{
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(
            ApiResult<IEnumerable<WeatherForecastResponseModel>>.Success(await _weatherForecastService.GetAsync()));
    }
}
