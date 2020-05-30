using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
//using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Web.Controllers
{

    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Auth(bool visible = true, ViewModel model = null)
        {
            ViewBag.visibleprivate = "none";
            model.UserInfoModel = new UserInfoModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Auth(ViewModel model,string action )
        {
            if (action == "SignUp")
            {
                if (!ModelValidator.Validate<UserInfoModel>(model.UserInfoModel))
                {
                    ViewBag.visibleprivate = "none";
                    return View(model);
                }
                ViewBag.visibleuser = "none";
                ModelState.Clear();
                model.PrivateUserInfoModel = new PrivateUserInfoModel();
                return View(model);
            }
            else
            {
                if (!ModelValidator.Validate<PrivateUserInfoModel>(model.PrivateUserInfoModel))
                {
                    ViewBag.visibleuser = "none";
                    return View(model);
                }
                return View("AuthComplete", model);
            }
        }
        public IActionResult AuthComplete(ViewModel model)
        {
            return View(model);
        }
    }
}



class ModelValidator
{
    public static bool Validate<T>(T model) where T : class, new()
    {
        model = model ?? new T();

        var validationContext = new ValidationContext(model);

        var validationResults = new List<ValidationResult>();

        Validator.TryValidateObject(model, validationContext, validationResults, true);
        if (validationResults.Count() == 0)
        {
            return true;
        }
        return false;
    }
}