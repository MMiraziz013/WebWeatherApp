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

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        public Condition Condition { get; set; }

        [JsonPropertyName("wind_dir")]
        public string WindDir {  get; set; }

        [JsonPropertyName("vis_km")]
        public double Visibility { get; set; }
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
        public List<Hour> Hour { get; set; }
    }

    public class Hour
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("temp_c")]
        public double TempC { get; set; }

        public Condition Condition { get; set; }
    }
}
