using Microsoft.AspNetCore.Mvc;

namespace BasicKubeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET /weatherforecast/temperature?lat=..&lon=..
        [HttpGet("temperature")]
        public async Task<double> GetTemperature([FromQuery] double lat, [FromQuery] double lon)
        {
            _logger.LogInformation("temperature called");
            return await _mediator.Send(new GetTempQuery { Lat = lat, Lon = lon });
        }
    }
}