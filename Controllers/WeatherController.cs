using Microsoft.AspNetCore.Mvc;

namespace WebWeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
