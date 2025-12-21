using BasicKube.Application.Queries;

using MediatR;

public static class WeatherEndpoints
{
    /// <summary>
    /// https://{base_ulr}/swagger/index.html
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapWeatherEndpoints(
        this IEndpointRouteBuilder app)
    {
        ///weatherforecast/temperature?lat=50.44&lon=30.52
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

        ////weatherforecast/wind?lat=50.44&lon=30.52
        app.MapGet(
            "/weatherforecast/wind",
            async (
                double lat,
                double lon,
                IMediator mediator,
                ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("WeatherEndpoints");
                logger.LogInformation("wind called");

                return await mediator.Send(new GetWindQuery
                {
                    Lat = lat,
                    Lon = lon
                });
            })
            .WithName("GetWind");

        return app;
    }
}
