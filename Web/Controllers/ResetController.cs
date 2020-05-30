using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using System.Security.Cryptography;
namespace Web.Controllers
{
    public class ResetController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.email = true;
            return View();
        }
        private static Random random = new Random();
        private string GetCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public IActionResult Email(ResetModel model, string submit)
        {
            if (submit == "send")
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.email = false;
            CodeModel modelCodeTemp = new CodeModel();
            modelCodeTemp.code = GetCode(5);
            return View("Index", modelCodeTemp);
        }
        public IActionResult Verify(CodeModel model)
        {
            if (!TryValidateModel(model,nameof(model)))
            {
                ViewBag.email = false;
                model.confirmCode = "";
                return View("Index", model);
            }
            return View("Reseted");
        }
    }
}