using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Landing()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLocation (string location)
        {
            if(string.IsNullOrEmpty(location))
            {
                ModelState.AddModelError("location", "Location is required");
                return View("Landing");
            }

            HttpContext.Session.SetString("Location", location);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var emptyWeather = new Weather
            //{
            //    Temperature = string.Empty,
            //    Icon = string.Empty,
            //    Condition = string.Empty,
            //    WindSpeed = string.Empty,
            //    Humidity = string.Empty,
            //    WindDirection = string.Empty,
            //    Location = "Unknown",
            //    Visibility = string.Empty,
            //};

            var location = HttpContext.Session.GetString("Location");

            // If no location is set in session, redirect to Landing
            if (string.IsNullOrWhiteSpace(location))
            {
                return RedirectToAction("Landing");
            }

            try
            {
                var weather = await _weatherService.GetWeatherWithHourlyAsync(location);
                return View(weather); // Pass weather data to the main weather view
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Could not retrieve weather data. Please try again.");
                return View(new Weather()); // Pass an empty model to avoid null reference errors
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetWeather(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                ModelState.AddModelError("location", "Location is required");
                return View("Index");
            }

            // Update session with the new location
            HttpContext.Session.SetString("Location", location);

            try
            {
                var weather = await _weatherService.GetWeatherWithHourlyAsync(location);
                return View("Index", weather); // Pass updated weather data
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Could not retrieve weather data. Please try again.");
                return View("Index");
            }
        }

    }
}
