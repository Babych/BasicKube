using BasicKube.Application.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace BasicKubeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        private readonly IMediator _mediator;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("GetTemperature")]
        public async Task<double> GetTemperature(
            [FromQuery] double lat,
            [FromQuery] double lon)
        {
            _logger.LogInformation("temperature");
            return await _mediator.Send(new GetTempQuery
            {
                Lat = lat,
                Lon = lon
            });
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<object> Get()
        {
            _logger.LogInformation("GetWeatherForecast");
            return Enumerable.Range(1, 5).Select(index => new
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
