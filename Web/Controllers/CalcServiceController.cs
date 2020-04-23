using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;

namespace Web.Controllers
{
    public class CalcServiceController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public CalcServiceController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult PassUsingModel()
        {
            ValueModel viewModel = new ValueModel();
            return View(viewModel);
        }
        public IActionResult PassUsingViewData()
        {
            ViewData["Value"] = new ValueModel();

            return View();
        }

        public IActionResult PassUsingViewBag()
        {
            ViewBag.Value = new ValueModel();
            return View();
        }

        public IActionResult AccessServiceDirectly()
        {
            ValueModel viewModel = new ValueModel();
            return View(viewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
