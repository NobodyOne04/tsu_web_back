using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Services;

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
            OperationService service = new OperationService();
            return View(service);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
