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
                        .Skip(1)
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

        public async Task<Weather> GetWeatherForWeek(string location)
        {
            try
            {
                string url = $"{BaseUrl}/forecast.json?key={ApiKey}&q={location}&days=3&aqi=no&alerts=no";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<WeatherApiResponse>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    if (weatherData == null || weatherData.Forecast == null || weatherData.Forecast.Forecastday == null || !weatherData.Forecast.Forecastday.Any())
                    {
                        throw new InvalidOperationException("Invalid response from Weather API.");
                    }

                    // Map the forecast data to a list of Weather objects
                    var weeklyWeather = new Weather
                    {
                        Location = weatherData.Location.Name,
                        DailyWeather = weatherData.Forecast.Forecastday.Select(forecast => new DailyWeather
                        {
                            Date = forecast.Date,
                            DayOfWeek = DateTime.Parse(forecast.Date).DayOfWeek.ToString(),
                            MaxTemperature = $"{forecast.Day.MaxTempC}°C",
                            MinTemperature = $"{forecast.Day.MinTempC}°C",
                            MaxWindSpeed = $"{forecast.Day.MaxWindKph}",
                            AvgHumidity = $"{forecast.Day.AvgHumidity}",
                            Condition = forecast.Day.Condition.Text,
                            Icon = forecast.Day.Condition.Icon,
                            ChanceOfRain = forecast.Day.DailyChanceOfRain,
                            ChanceOfSnow = forecast.Day.DailyChanceOfSnow,
                            Sunrise = forecast.Astro.Sunrise,
                            Sunset = forecast.Astro.Sunset,

                        }).ToList()
                    };


                    return weeklyWeather;
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

    public static class WeatherImageHelper
    {
        public static string GetWeatherGif(string condition)
        {
            string picType = string.Empty;

            if (condition.Contains("Rain") || condition.Contains("rain") || condition.Contains("drizzle") || condition.Contains("Drizzle"))
            {
                picType = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExMTB6YTZldGlodzQ0YnpzN3liajJqYTI4d2F0ejByMWV1ZDduaGZwYiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/t7Qb8655Z1VfBGr5XB/giphy.gif";
            }
            else if (condition.Contains("Cloud") || condition.Contains("cloud"))
            {
                picType = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExZTJ0aXFhZmZmZzVjajNiZ2NqOWlieWtkYnB5dHJkcGxnaGlkNDZnNSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/lOkbL3MJnEtHi/giphy.gif";
            }
            else if (condition.Contains("Sunny") || condition.Contains("sunny"))
            {
                picType = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExamQ2emM2aGJyZGVwMGFodDhncGtxNHMzY3FnZWd0OWo2bGd0MHh5NiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/igtTYUrBA77vW/giphy.gif";
            }
            else if (condition.Contains("Snow") || condition.Contains("snow"))
            {
                picType = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExODlhMDZsOWZoajVwcjhpNGhkd2FhMW0zOGl0dzdrYTF2ajJwN295ciZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/12UCxsXdazBCso/giphy.gif";
            }
            else if (condition.Contains("Overcast") || condition.Contains("overcast"))
            {
                picType = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExbjZvY3J6ejRtYmh6NjY2aDdiOXBhYnZnc2Q0cnhwdTU3MnZ2em9kZSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/IZtRbZKQBCA3m/giphy.gif";
            }
            else if (condition.Contains("Mist") || condition.Contains("mist"))
            {
                picType = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExdW5qMHBmYjVmd29wM2oydTMyZzlvZ3YxZXpmZmhxYmwwYTg0MDUxNSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/uf3jumi0zzUv6/giphy.gif";
            }
            else if (condition.Contains("Fog") || condition.Contains("fog"))
            {
                picType =  "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExemZyd2lydTMxYnhnb2l6YXBpNmZ5eDByeDA5M3ozNWx4YXV6c2kzaSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/xEjTM5COAKyNa/giphy.gif";
            }
            else if (condition.Contains("Storm") || condition.Contains("storm"))
            {
                picType = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExcmMyNWE2ZjNraGIwbjNjYTlvOHVheWgwY3NlZjVwdDF6ZjFpYThwZCZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/xTcnT45z6H";
            }
            else
            {
               picType = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExamQ2emM2aGJyZGVwMGFodDhncGtxNHMzY3FnZWd0OWo2bGd0MHh5NiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/igtTYUrBA77vW/giphy.gif";
            }

            return picType;
        }
    }

}