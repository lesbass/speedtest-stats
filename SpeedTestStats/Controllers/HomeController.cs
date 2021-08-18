using Microsoft.AspNetCore.Mvc;
using SpeedTestStats.BL.Interfaces;

namespace SpeedTestStats.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatsReader _statsReader;

        public HomeController(IStatsReader statsReader)
        {
            _statsReader = statsReader;
        }

        public IActionResult Index()
        {
            return View(_statsReader.GetRows());
        }
    }
}