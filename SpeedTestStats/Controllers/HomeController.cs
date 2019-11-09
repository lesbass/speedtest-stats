using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpeedTestStats.BL;
using SpeedTestStats.Models;

namespace SpeedTestStats.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly StatsReader _statsReader;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
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

      //return Content(item.OrderByDescending(x => x.DataOra).Take(5).ToJSON());
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
