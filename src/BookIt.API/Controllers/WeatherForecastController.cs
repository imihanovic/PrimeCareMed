using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookIt.Application.Models;
using BookIt.Application.Models.WeatherForecast;
using BookIt.Application.Services;

namespace BookIt.API.Controllers;

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
