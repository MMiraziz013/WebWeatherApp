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
            var location = HttpContext.Session.GetString("Location");

            // If no location is set in session, redirect to Landing
            if (string.IsNullOrWhiteSpace(location))
            {
                return RedirectToAction("Landing");
            }

            try
            {
                var weather = await _weatherService.GetWeatherWithHourlyAsync(location);

                // Get the current hour
                var currentTime = DateTime.Now;

                // Add CurrentHour property to the Weather model
                if (weather.HourlyWeather != null)
                {
                    weather.CurrentHour = currentTime.ToString("HH:mm"); // Add formatted time
                }

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

        [HttpPost]
        public async Task<IActionResult> GetTomorrowWeather(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                ModelState.AddModelError("location", "Location is required");
                return View("Index");
            }

            HttpContext.Session.SetString("Location", location);

            try
            {
                var weather = await _weatherService.GetWeatherForTomorrow(location);
                return View("Tomorrow", weather);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Could not retrieve weather data. Please try again.");
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetWeeksWeather(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                ModelState.AddModelError("location", "Location is required");
                return View("Index");
            }

            HttpContext.Session.SetString("Location", location);

            try
            {
                var weeklyWeather = await _weatherService.GetWeatherForWeek(location);
                return View("Week", weeklyWeather);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Could not retrieve weather data. Please try again.");
                return View("Index");
            }
        }   
    }
}