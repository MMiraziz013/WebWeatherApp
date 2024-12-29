using System.Text.Json.Serialization;

namespace WebWeatherApp.Models
{
    public class WeatherApiResponse
    {
        public Location Location { get; set; }
        public CurrentWeather Current {  get; set; }

        public Forecast Forecast { get; set; }
    }

    public class CurrentWeather
    {
        [JsonPropertyName("temp_c")]
        public double TempC { get; set; }

        [JsonPropertyName("wind_kph")]
        public double WindKph { get; set; }

        [JsonPropertyName("precip_in")]
        public double Precipitation_mm { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        public Condition Condition { get; set; }

        [JsonPropertyName("wind_dir")]
        public string WindDir {  get; set; }

        [JsonPropertyName("vis_km")]
        public double Visibility { get; set; }

        [JsonPropertyName("uv")]
        public double UvIndex { get; set; }

        [JsonPropertyName("is_day")]
        public int DayNight { get; set; }

    }

    public class Condition
    {
        public string Text { get; set; }
        public string Icon { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
    }
    public class Forecast
    {
        public List<ForecastDay> Forecastday { get; set; }
    }

    public class ForecastDay
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("day")]
        public Day Day { get; set; }

        public List<Hour> Hour { get; set; }

        [JsonPropertyName("astro")]
        public Astro Astro { get; set; }
    }

    public class Day
    {
        [JsonPropertyName("maxtemp_c")]
        public double MaxTempC { get; set; }

        [JsonPropertyName("mintemp_c")]
        public double MinTempC { get; set; }

        [JsonPropertyName("avgtemp_c")]
        public double AvgTempC { get; set; }

        [JsonPropertyName("maxwind_kph")]
        public double MaxWindKph { get; set; }

        [JsonPropertyName("totalprecip_mm")]
        public double TotalPrecipMm { get; set; }

        [JsonPropertyName("avgvis_km")]
        public double AvgVisKm { get; set; }

        [JsonPropertyName("avghumidity")]
        public int AvgHumidity { get; set; }

        [JsonPropertyName("daily_will_it_rain")]
        public int DailyWillItRain { get; set; }

        [JsonPropertyName("daily_chance_of_rain")]
        public int DailyChanceOfRain { get; set; }

        [JsonPropertyName("daily_will_it_snow")]
        public int DailyWillItSnow { get; set; }

        [JsonPropertyName("daily_chance_of_snow")]
        public int DailyChanceOfSnow { get; set; }

        public Condition Condition { get; set; }

        [JsonPropertyName("uv")]
        public double Uv { get; set; }
    }

    public class Hour
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("temp_c")]
        public double TempC { get; set; }

        public Condition Condition { get; set; }
    }
    public class Astro
    {
        [JsonPropertyName("sunrise")]
        public string Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public string Sunset { get; set; }

        [JsonPropertyName("moonrise")]
        public string Moonrise { get; set; }

        [JsonPropertyName("moonset")]
        public string Moonset { get; set; }

        [JsonPropertyName("moon_phase")]
        public string MoonPhase { get; set; }

        [JsonPropertyName("moon_illumination")]
        public int MoonIllumination { get; set; }

        [JsonPropertyName("is_moon_up")]
        public int IsMoonUp { get; set; }

        [JsonPropertyName("is_sun_up")]
        public int IsSunUp { get; set; }

        [JsonPropertyName("avgvis_km")]
        public double AverageVisibility { get; set; }
    }

}