using BasicKube.Application.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace BasicKubeApi.Controllers
{
    [ApiController]
    [Route("weatherforecast")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WeatherForecastController> _logger;

        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET /weatherforecast/temperature?lat=..&lon=..
        [HttpGet("temperature")]
        public async Task<double> GetTemperature([FromQuery] double lat, [FromQuery] double lon)
        {
            _logger.LogInformation("GetTemperature called");
            return await _mediator.Send(new GetTempQuery { Lat = lat, Lon = lon });
        }

        // GET /weatherforecast/g1
        [HttpGet("g1")]
        public IEnumerable<string> GetG1()
        {
            _logger.LogInformation("GetG1 called");
            return new[] { "GetG1" };
        }

        // GET /weatherforecast/g2
        [HttpGet("g2")]
        public async Task<IEnumerable<string>> GetG2()
        {
            _logger.LogInformation("GetG2 called");
            await Task.CompletedTask;
            return new[] { "GetG2" };
        }

        // GET /weatherforecast/g3?g3=value
        [HttpGet("g3")]
        public IEnumerable<string> GetG3([FromQuery] string g3)
        {
            _logger.LogInformation("GetG3 called");
            return new[] { g3 };
        }

        // GET /weatherforecast/g4?g4=value
        [HttpGet("g4")]
        public async Task<IEnumerable<string>> GetG4([FromQuery] string g4)
        {
            _logger.LogInformation("GetG4 called");
            await Task.CompletedTask;
            return new[] { g4 };
        }

        // GET /weatherforecast
        [HttpGet]
        public IEnumerable<object> GetWeatherForecast()
        {
            _logger.LogInformation("GetWeatherForecast called");

            return Enumerable.Range(1, 5).Select(index => new
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
        }
    }
}