using Backend6.Application;
using Backend6.Core;
using Backend6.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.StaticFiles;

namespace Web;

/// <summary>The signed-in user as the domain's Actor (null when anonymous).</summary>
public class ActorAccessor(IHttpContextAccessor http, UserManager<ApplicationUser> users)
{
    public Actor? Current
    {
        get
        {
            var user = http.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated != true) return null;
            return new Actor(users.GetUserId(user)!, user.Identity.Name ?? "", user.IsInRole(Roles.Administrators));
        }
    }
}

/// <summary>Domain outcomes as HTTP: not found → 404; forbidden → 403, or the login page when anonymous.</summary>
public class DomainErrorsFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            NotFoundException => new NotFoundResult(),
            ForbiddenException when context.HttpContext.User.Identity?.IsAuthenticated != true => new ChallengeResult(),
            ForbiddenException => new ForbidResult(),
            _ => null,
        };
        context.ExceptionHandled = context.Result is not null;
    }
}

/// <summary>Shared controller helpers.</summary>
public abstract class AppController : Controller
{
    private static readonly FileExtensionContentTypeProvider Types = new();

    protected Actor? Actor => HttpContext.RequestServices.GetRequiredService<ActorAccessor>().Current;

    /// <summary>Run a write; a broken domain rule goes back to the form as a validation error.</summary>
    protected async Task<IActionResult> SubmitAsync(Func<Task<IActionResult>> action, Func<IActionResult> showForm)
    {
        if (!ModelState.IsValid) return showForm();
        try
        {
            return await action();
        }
        catch (DomainException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return showForm();
        }
    }

    /// <summary>Images display inline; anything else downloads, so uploaded HTML or SVG never runs here.</summary>
    protected IActionResult Send(Download d)
    {
        Response.Headers.XContentTypeOptions = "nosniff";
        if (d.IsImage && Types.TryGetContentType(d.FileName, out var type)) return File(d.Content, type);
        return File(d.Content, "application/octet-stream", d.FileName);
    }

    protected static Upload? ToUpload(IFormFile? file) =>
        file is null ? null : new Upload(file.FileName, file.Length, file.OpenReadStream);

    protected IActionResult NameForm(Forms.NameForm model, string heading, string back) =>
        View("NameForm", model).With(ViewData, heading, back);

    protected IActionResult TextForm(Forms.TextForm model, string heading, string back) =>
        View("TextForm", model).With(ViewData, heading, back);

    protected IActionResult UploadForm(string heading, string back) =>
        View("UploadForm", new Forms.UploadForm()).With(ViewData, heading, back);

    protected IActionResult ConfirmDelete(string what, string back) =>
        View("ConfirmDelete", new ConfirmDeleteModel(what, back));
}

public sealed record ConfirmDeleteModel(string What, string Back);

internal static class ViewExtensions
{
    public static IActionResult With(this ViewResult view, Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary data, string heading, string back)
    {
        data["Title"] = heading;
        data["Back"] = back;
        return view;
    }
}
