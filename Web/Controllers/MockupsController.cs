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
            SignUp model = new SignUp();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(SignUp model)
        {
            String Day = this.Request.Form["Day"];
            String Month = this.Request.Form["Month"];
            String Year = this.Request.Form["Year"];
            model.born = RegistrationService.BornDate(Day, Month, Year);

            if (RegistrationService.TryToFind(model)) return this.View("SignUpAlreadyExists", model);

            SignIn modelsec = new SignIn();
            modelsec.born = model.born;
            modelsec.FName = model.FName;
            modelsec.gender = model.gender;
            modelsec.SName = model.SName;
            return this.View("SignUpCredentials", modelsec);
        }

        public IActionResult SignUpAlreadyExists(SignUp model)
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpAlreadyExists(SignUp model, String action)
        {
            if (action == "Sign") {
                SignIn modelsec = new SignIn();
                modelsec.born = model.born;
                modelsec.FName = model.FName;
                modelsec.gender = model.gender;
                modelsec.SName = model.SName;
                return this.View("SignUpCredentials", modelsec);
            }
            model = new SignUp();
            return this.View("SignUp", model);
        }

        [HttpGet]
        public IActionResult SignUpCredentials(SignIn model)
        {
            return this.View(model);
        }

        [HttpPost, ActionName("SignUpCredentials")]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpCredentialsConfirm(SignIn model)
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
        public IActionResult SignUpResult(SignIn model)
        {
            return this.View();
        }

        private void ValidatePassword(SignIn model)
        {
            if (model.Password != model.ConfPassword) this.ModelState.AddModelError("Password", "Passwords don't the same");
        }
    }
}