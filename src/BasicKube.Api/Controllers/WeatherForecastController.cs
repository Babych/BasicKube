using BasicKube.Application.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace BasicKubeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("temperature")]
        public async Task<double> GetTemperature(
            [FromQuery] double lat,
            [FromQuery] double lon)
        {
            return await _mediator.Send(new GetTempQuery
            {
                Lat = lat,
                Lon = lon
            });
        }
    }
}
