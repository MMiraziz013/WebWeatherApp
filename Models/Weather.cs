namespace WebWeatherApp.Models;

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

    public int? DayOrNight { get; set; }

    public List<HourlyWeather>? HourlyWeather { get; set; }

    public string? CurrentHour { get; set; }

    // New properties for the daily forecast
    public string? Date { get; set; }
    public string? MaxTemperature { get; set; }
    public string? MinTemperature { get; set; }
    public string? AvgTemperature { get; set; }
    public string? MaxWindSpeed { get; set; }
    public string? AvgHumidity { get; set; }

    public string? AverageVisibility { get; set; }
    public string? ChanceOfRain { get; set; }
    public string? ChanceOfSnow { get; set; }

    public string? Sunrise { get; set; }
    public string? Sunset { get; set; }

}
