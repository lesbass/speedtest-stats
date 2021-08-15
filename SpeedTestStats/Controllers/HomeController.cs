using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SpeedTestStats.BL;

namespace SpeedTestStats.Controllers
{
    public class HomeController : Controller
    {
        private readonly StatsReader _statsReader;

        public HomeController()
        {
            _statsReader = new StatsReader();
        }

        public IActionResult Index()
        {
            var tempi = new List<double>();
            var item = new List<StatRow>();
            for (var i = 0; i < 1; i++)
            {
                var t = DateTime.Now;
                item = _statsReader.Get();

                tempi.Add(DateTime.Now.Subtract(t).TotalMilliseconds);
            }

            ViewBag.ProcTime = tempi.Average();

            return View(item);
        }
    }
}