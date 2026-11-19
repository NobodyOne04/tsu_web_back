using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class MockupsController : Controller
    {
        [HttpGet]
        public IActionResult Quiz()
        {
            Operations operations = new Operations();
            return View(operations);
        }

        [HttpPost]
        public IActionResult Quiz(Operations operation, string action)
        {
            double number;
            if (double.TryParse(operation.YourAnswer, out number) & ModelState.IsValid)
            {
                CorrectAnswers answers = CorrectAnswers.Instance;
                answers.Total+= 1;
                operation.Solution(); 
                ViewData["NotANumber"] = "" + operation.First + 
                    "  ||  " + operation.Second;
                if (operation.Check())
                    answers.Correct += 1;
                (answers.Answers).Add(operation);
            }
            else
            {
                ViewData["NotANumber"] = "Not a number!";
                return View(operation);
            }

            if (action == "Next")
                return View(new Operations());
            return RedirectToAction("QuizResult");
        }

        public IActionResult QuizResult()
        {
            CorrectAnswers answers = CorrectAnswers.Instance;
            ViewBag.Result = answers.Answers;
            ViewData["Correct"] = "" + answers.Correct;
            ViewData["Total"] = "" + answers.Total;
            return View();
        }
    }
}