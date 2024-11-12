﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebWeatherApp.Models;
using WebWeatherApp.Services;

namespace WebWeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var emptyWeather = new Weather
            {
                Temperature = string.Empty,
                Icon = string.Empty,
                Condition = string.Empty,
                WindSpeed = string.Empty,
                Humidity = string.Empty,
                WindDirection = string.Empty,
                Location = "Unknown",
                Visibility = string.Empty,
            };

            return View(emptyWeather);
        }

        [HttpPost]
        public async Task<IActionResult> GetWeather(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                ModelState.AddModelError("location", "Location is required");
                return View("Index");
            }

            try
            {
                var weather = await _weatherService.GetWeatherAsync(location);
                return View("Index", weather); // Pass the updated weather data, including location
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Could not retrieve weather data. Please try again.");
                return View("Index");
            }
        }
    }
}
