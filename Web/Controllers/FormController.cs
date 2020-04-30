using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
public class FormController : Controller
    {
        
        public ViewResult ModelBindPar()
        {
            FormModel model = new FormModel();
            return View(model);
        }
        
        [HttpPost]
        public IActionResult ModelBindPar(int first, string operation, int second)
        {
            FormModel model = new FormModel(first, second,operation);
            if (!TryValidateModel(model, nameof(model)))
            {
                return View(model);
            }
            model.Calculate();
            return View("ResultFormModel", model);
        }
        
        public ViewResult ModelBindSep()
        {
            FormModel model = new FormModel();
            return View(model);
        }
        
        [HttpPost]
        public IActionResult ModelBindSep(FormModel model)
        {
            if (!TryValidateModel(model, nameof(model))){
                return View(model);
            }
            model.Calculate();
            return View("ResultFormModel", model);
        }

        [HttpGet]
        public IActionResult ManualSep()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult ManualSep(int first, string operation, int second)
        {
            int result = FormModel().Calculate(first, second, operation);
            ViewBag.expression = $"{first} {operation} {second} = {result}";
            return View("ResultForm");
        }
        
        public IActionResult ManualSingle (int first, string operation, int second)
        {
            if(Request.Method == "GET")
            {
                return View();
            }
            if (Request.Method == "POST")
            {
                int result = this.calculate(first, second, operation);
                ViewBag.expression = $"{first} {operation} {second} = {result}";
                return View("ResultForm");
            }
            return View();
        }
    }
}