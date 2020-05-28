using Web.Models;
using Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers
{
    public class MockupsController : Controller
    {
        private IRegistrationService RegistrationService;

        public MockupsController(IRegistrationService par) {
            RegistrationService = par;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult SignUp()
        {
            SignUpFst model = new SignUpFst();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(SignUpFst model)
        {
            String Day = this.Request.Form["Day"];
            String Month = this.Request.Form["Month"];
            String Year = this.Request.Form["Year"];
            model.born = RegistrationService.BornDate(Day, Month, Year);

            if (RegistrationService.TryToFind(model)) return this.View("SignUpAlreadyExists", model);

            SignUpSec modelsec = new SignUpSec();
            modelsec.born = model.born;
            modelsec.FName = model.FName;
            modelsec.gender = model.gender;
            modelsec.SName = model.SName;
            return this.View("SignUpCredentials", modelsec);
        }

        public IActionResult SignUpAlreadyExists(SignUpFst model)
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpAlreadyExists(SignUpFst model, String action)
        {
            if (action == "Sign") {
                SignUpSec modelsec = new SignUpSec();
                modelsec.born = model.born;
                modelsec.FName = model.FName;
                modelsec.gender = model.gender;
                modelsec.SName = model.SName;
                return this.View("SignUpCredentials", modelsec);
            }
            model = new SignUpFst();
            return this.View("SignUp", model);
        }

        [HttpGet]
        public IActionResult SignUpCredentials(SignUpSec model)
        {
            return this.View(model);
        }

        [HttpPost, ActionName("SignUpCredentials")]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpCredentialsConfirm(SignUpSec model)
        {
            ValidatePassword(model);
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            RegistrationService.SendPerson(model);
            return this.View("SignUpResult", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpResult(SignUpSec model)
        {
            return this.View();
        }

        private void ValidatePassword(SignUpSec model)
        {
            if (model.Password != model.ConfPassword) this.ModelState.AddModelError("Password", "Passwords don't the same");
        }
    }
}