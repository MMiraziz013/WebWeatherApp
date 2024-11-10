using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using WebWeatherApp.Models;

namespace WebWeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "eedaac4e934f4014aef62441240511";
        private const string BaseUrl = "http://api.weatherapi.com/v1";

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Weather> GetWeatherAsync (string location)
        {
            try
            {
                string url = $"{BaseUrl}/current.json?key={ApiKey}&q={location}&aqi=no";

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync ();
                    var weatherData = JsonSerializer.Deserialize<WeatherApiResponse> (jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    if (weatherData == null || weatherData.Current == null)
                    {
                        throw new InvalidOperationException("Invalid response due to Weather API.");
                    }

                    // EDIT THE WEATHER CONTROLLER, AND THEN FIX THIS PART.
                    var weather = new Weather
                    {
                        Temperature = $"{weatherData.Current.TempC}°C",
                        Condition = weatherData.Current.Condition.Text,
                        Icon = weatherData.Current.Condition.Icon,
                        WindSpeed = $"{weatherData.Current.WindKph} kph",
                        Humidity = $"{weatherData.Current.Humidity}%",
                        WindDirection = weatherData.Current.WindDir,
                        Visibility = $"{weatherData.Current.Visibility} km",
                    };
                    return weather;
                }
                else
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured whuile fetching the weather data", ex);
            }
        }
    }
}