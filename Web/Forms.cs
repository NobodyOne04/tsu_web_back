using System.ComponentModel.DataAnnotations;
using Backend6.Core;
using Microsoft.AspNetCore.Http;

namespace Web.Forms;

// What the views post. Field checks give quick feedback; the domain enforces the real rules.

public class LoginForm
{
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
    [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
}

public class RegisterForm
{
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)] public string Password { get; set; } = "";
    [DataType(DataType.Password), Display(Name = "Confirm password"), Compare(nameof(Password), ErrorMessage = "The passwords don't match.")]
    public string ConfirmPassword { get; set; } = "";
}

public class ChangePasswordForm
{
    [Required, DataType(DataType.Password), Display(Name = "Current password")] public string OldPassword { get; set; } = "";
    [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password), Display(Name = "New password")] public string NewPassword { get; set; } = "";
    [DataType(DataType.Password), Display(Name = "Confirm new password"), Compare(nameof(NewPassword), ErrorMessage = "The passwords don't match.")]
    public string ConfirmPassword { get; set; } = "";
}

public class NameForm
{
    [Required, MaxLength(Rules.MaxName)] public string Name { get; set; } = "";
}

public class TextForm
{
    [Required, MaxLength(10_000), DataType(DataType.MultilineText)] public string Text { get; set; } = "";
}

public class UploadForm
{
    [Required(ErrorMessage = "Choose a file.")] public IFormFile? File { get; set; }
}

public class PostForm
{
    [Display(Name = "Category")] public Guid CategoryId { get; set; }
    [Required, MaxLength(Rules.MaxName)] public string Title { get; set; } = "";
    [Required, DataType(DataType.MultilineText)] public string Text { get; set; } = "";
}

public class ForumForm
{
    [Display(Name = "Category")] public Guid CategoryId { get; set; }
    [Required, MaxLength(Rules.MaxName)] public string Name { get; set; } = "";
    [DataType(DataType.MultilineText)] public string? Description { get; set; }
}

public class TopicForm
{
    [Required, MaxLength(Rules.MaxName)] public string Name { get; set; } = "";
    [Required, MaxLength(10_000), DataType(DataType.MultilineText), Display(Name = "First message")] public string Text { get; set; } = "";
}
