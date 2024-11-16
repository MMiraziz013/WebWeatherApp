namespace WebWeatherApp.Models
{
    public class HourlyWeather
    {
        public string? Time { get; set; }
        public string? Temperature { get; set; }
        public string? ConditionText { get; set; }
        public string? ConditionIcon { get; set; }
    }
}
