using Backend5.Core;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

/// <summary>Shared POST handling: a broken domain rule goes back to the form as a validation error.</summary>
public abstract class FormController : Controller
{
    protected async Task<IActionResult> SubmitAsync(Func<Task> action, Func<IActionResult> onSuccess, Func<IActionResult> showForm)
    {
        if (!ModelState.IsValid) return showForm();
        try
        {
            await action();
        }
        catch (DomainException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return showForm();
        }
        return onSuccess();
    }
}
