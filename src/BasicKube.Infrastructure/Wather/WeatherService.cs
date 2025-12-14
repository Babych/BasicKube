using System.Net.Http.Json;

using BasicKube.Application.Queries;

namespace BasicKube.Infrastructure.Wather
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _http;

        public WeatherService(HttpClient http)
        {
            _http = http;
        }

        public async Task<double> GetCurrentTemperatureAsync(double lat, double lon)
        {
            string url =
                $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m";

            var result = await _http.GetFromJsonAsync<WeatherApiResponse>(url);

            return result!.Current.Temperature_2m;
        }
    }
}
