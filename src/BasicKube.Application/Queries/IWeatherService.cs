namespace BasicKube.Application.Queries
{
    public interface IWeatherService
    {
        Task<double> GetCurrentTemperatureAsync(double lat, double lon);
    }
}
