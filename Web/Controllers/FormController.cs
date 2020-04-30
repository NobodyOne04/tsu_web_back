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
            model.Result = FormModel.Calculate(first, second, operation);
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
            model.Result = FormModel.Calculate(model.First, model.Second, model.Operation);
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
            float result = FormModel.Calculate(first, second, operation);
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
                float result = FormModel.Calculate(first, second, operation);
                ViewBag.expression = $"{first} {operation} {second} = {result}";
                return View("ResultForm");
            }
            return View();
        }
    }
}