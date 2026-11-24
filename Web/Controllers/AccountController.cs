using Backend6.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Forms;

namespace Web.Controllers;

public class AccountController(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn) : Controller
{
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginForm());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginForm model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(model);
        var result = await signIn.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
        if (result.Succeeded) return LocalRedirect(Url.IsLocalUrl(returnUrl) ? returnUrl! : "/");
        ModelState.AddModelError(string.Empty, result.IsLockedOut ? "Too many attempts; try again later." : "Wrong email or password.");
        return View(model);
    }

    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new RegisterForm());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterForm model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(model);
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await users.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await signIn.SignInAsync(user, isPersistent: false);
            return LocalRedirect(Url.IsLocalUrl(returnUrl) ? returnUrl! : "/");
        }
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> Logout()
    {
        await signIn.SignOutAsync();
        return Redirect("/");
    }
}

[Authorize]
public class ManageController(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn) : Controller
{
    public IActionResult Index(string? message = null)
    {
        ViewData["StatusMessage"] = message;
        return View();
    }

    public IActionResult ChangePassword() => View(new ChangePasswordForm());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordForm model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await users.GetUserAsync(User) ?? throw new InvalidOperationException("Signed-in user not found.");
        var result = await users.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (result.Succeeded)
        {
            await signIn.RefreshSignInAsync(user);
            return RedirectToAction(nameof(Index), new { message = "Your password has been changed." });
        }
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        return View(model);
    }
}

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Posts");

    public IActionResult Error() => View();
}
