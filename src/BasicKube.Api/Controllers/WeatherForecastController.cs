using BasicKube.Application.Queries;

using MediatR;

public static class WeatherEndpoints
{
    /// <summary>
    /// https://{base_url}/WeatherForecast/temperature?lat=50.44&lon=30.52
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapWeatherEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/weatherforecast/temperature",
            async (
                double lat,
                double lon,
                IMediator mediator,
                ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("WeatherEndpoints");
                logger.LogInformation("temperature called");

                return await mediator.Send(new GetTempQuery
                {
                    Lat = lat,
                    Lon = lon
                });
            })
            .WithName("GetTemperature");

        return app;
    }
}
