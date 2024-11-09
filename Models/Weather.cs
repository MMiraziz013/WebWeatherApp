namespace WebWeatherApp.Models
{
    public class Weather
    {
        public string? Temperature { get; set; }
        // Description of current weather (e.g., "Sunny", "Cloudy")
        public string? Condition { get; set; }
        public string? Icon {  get; set; }
        public string? WindDirection { get; set; }
        public string? WindSpeed { get; set; }
        public string? Humidity { get; set; }
        public string? Location {  get; set; }
    }
}
