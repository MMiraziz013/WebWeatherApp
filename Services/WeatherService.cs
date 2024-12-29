using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using WebWeatherApp.Models;
using System.Collections.Generic;

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
        public async Task<Weather> GetWeatherWithHourlyAsync(string location)
        {
            try
            {
                string url = $"{BaseUrl}/forecast.json?key={ApiKey}&q={location}&days=1&aqi=no&alerts=no";

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<WeatherApiResponse>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    if (weatherData == null || weatherData.Current == null || weatherData.Forecast == null)
                    {
                        throw new InvalidOperationException("Invalid response from Weather API.");
                    }

                    // Map hourly data to a simplified model
                    var hourlyData = weatherData.Forecast.Forecastday
                        .FirstOrDefault()?
                        .Hour.Select(hour => new HourlyWeather
                        {
                            // Parse and format the time to HH:mm
                            Time = DateTime.Parse(hour.Time).ToString("HH:mm"),
                            Temperature = $"{hour.TempC}°C",
                            ConditionText = hour.Condition.Text,
                            ConditionIcon = hour.Condition.Icon
                        }).ToList();

                    var currentTime = DateTime.Now.ToString("HH:mm");
                    var weather = new Weather
                    {
                        Temperature = $"{weatherData.Current.TempC}°C",
                        Condition = weatherData.Current.Condition.Text,
                        Icon = weatherData.Current.Condition.Icon,
                        WindSpeed = $"{weatherData.Current.WindKph} kph",
                        Humidity = $"{weatherData.Current.Humidity}%",
                        WindDirection = weatherData.Current.WindDir,
                        Visibility = $"{weatherData.Current.Visibility} km",
                        Location = weatherData.Location.Name,
                        HourlyWeather = hourlyData,
                        CurrentHour = currentTime,
                        Precipitation = $"{weatherData.Current.Precipitation_mm}",
                        Uv = $"{weatherData.Current.UvIndex}",
                        DayOrNight = weatherData.Current.DayNight,
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
                throw new ApplicationException("An error occurred while fetching the weather data", ex);
            }
        }

        public async Task<Weather> GetWeatherForTomorrow(string location)
        {
            try
            {
                string url = $"{BaseUrl}/forecast.json?key={ApiKey}&q={location}&days=2&aqi=no&alerts=no";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<WeatherApiResponse>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    if (weatherData == null || weatherData.Forecast == null || weatherData.Forecast.Forecastday.Count < 2)
                    {
                        throw new InvalidOperationException("Invalid response from Weather API.");
                    }

                    var tomorrowForecast = weatherData.Forecast.Forecastday[1];

                    var hourlyData = weatherData.Forecast.Forecastday
                        .FirstOrDefault()?
                        .Hour.Select(hour => new HourlyWeather
                        {
                            // Parse and format the time to HH:mm
                            Time = DateTime.Parse(hour.Time).ToString("HH:mm"),
                            Temperature = $"{hour.TempC}°C",
                            ConditionText = hour.Condition.Text,
                            ConditionIcon = hour.Condition.Icon
                        }).ToList();

                    var weather = new Weather
                    {
                        Location = weatherData.Location.Name,
                        Date = tomorrowForecast.Date,
                        MaxTemperature = $"{tomorrowForecast.Day.MaxTempC}°C",
                        MinTemperature = $"{tomorrowForecast.Day.MinTempC}°C",
                        AvgTemperature = $"{tomorrowForecast.Day.AvgTempC}°C",
                        MaxWindSpeed = $"{tomorrowForecast.Day.MaxWindKph} kph",
                        Precipitation = $"{tomorrowForecast.Day.TotalPrecipMm} mm",
                        AvgHumidity = $"{tomorrowForecast.Day.AvgHumidity}%",
                        ChanceOfRain = $"{tomorrowForecast.Day.DailyChanceOfRain}%",
                        Sunrise = $"{tomorrowForecast.Astro.Sunrise}",
                        Sunset = $"{tomorrowForecast.Astro.Sunset}",
                        AverageVisibility = $"{tomorrowForecast.Day.AvgVisKm}",
                        ChanceOfSnow = $"{tomorrowForecast.Day.DailyChanceOfSnow}%",
                        Condition = tomorrowForecast.Day.Condition.Text,
                        Icon = tomorrowForecast.Day.Condition.Icon,
                        Uv = $"{tomorrowForecast.Day.Uv}",
                        HourlyWeather = hourlyData,
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
                throw new ApplicationException("An error occurred while fetching the weather data", ex);
            }
        }

    }
}