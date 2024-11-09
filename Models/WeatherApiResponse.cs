namespace WebWeatherApp.Models
{
    public class WeatherApiResponse
    {
        public CurrentWeather Current {  get; set; }
    }

    public class CurrentWeather
    {
        public double TempC { get; set; }
        public double WindKph { get; set; }
        public int Humidity { get; set; }
        public Condition Condition { get; set; }
        public string WindDir {  get; set; }
    }

    public class Condition
    {
        public string Text { get; set; }
        public string Icon { get; set; }
    }
}
