﻿namespace WebWeatherApp.Models
{
    public class Weather
    {
        public string? Temperature { get; set; }
        public string? Condition { get; set; }
        public string? Icon { get; set; }
        public string? WindDirection { get; set; }
        public string? WindSpeed { get; set; }
        public string? Humidity { get; set; }
        public string? Location { get; set; }
        public string? Visibility { get; set; }
        public string? Precipitation { get; set; }
        public string? Uv { get; set; }

        public List<HourlyWeather>? HourlyWeather { get; set; }

        public string? CurrentHour { get; set; }
    }
}
