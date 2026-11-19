namespace Backend6.Core;

/// <summary>A broken rule; the message is shown to the user.</summary>
public class DomainException(string message) : Exception(message);

public class NotFoundException(string what, Guid id) : Exception($"{what} {id} not found");

/// <summary>The actor may not do this to that.</summary>
public class ForbiddenException(string message) : Exception(message);

/// <summary>Who is acting: the signed-in user as the domain sees them.</summary>
public sealed record Actor(string UserId, string UserName, bool IsAdmin)
{
    /// <summary>Owners and admins may change a thing.</summary>
    public bool Owns(string ownerId) => IsAdmin || UserId == ownerId;
}

public static class Rules
{
    public const int MaxName = 200;
    public const int MaxFileName = 255;
    public const long MaxUploadBytes = 10 * 1024 * 1024;

    public static string Required(string? value, string field, int max = MaxName)
    {
        var trimmed = value?.Trim() ?? "";
        if (trimmed.Length == 0) throw new DomainException($"{field} is required.");
        if (trimmed.Length > max) throw new DomainException($"{field} must be at most {max} characters.");
        return trimmed;
    }

    public static string? Optional(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    public static void Allow(bool allowed, string what)
    {
        if (!allowed) throw new ForbiddenException($"You can't {what}.");
    }

    public static Actor SignedIn(Actor? actor, string what) =>
        actor ?? throw new ForbiddenException($"Sign in to {what}.");

    public static void Admin(Actor? actor, string what) => Allow(actor?.IsAdmin == true, what);

    /// <summary>A file name as uploaded: just the name part, required, short enough.</summary>
    public static string FileName(string? name) =>
        Required(Path.GetFileName((name ?? "").Replace('\\', '/')), "File name", MaxFileName);
}

/// <summary>A stored upload: what the user called it and the key the file store uses.</summary>
public abstract class Attachment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string FileName { get; private set; } = "";
    public string StoredName { get; private set; } = "";
    public DateTime Created { get; private set; }

    protected Attachment() { }

    protected Attachment(string fileName, string storedName, DateTime now) =>
        (FileName, StoredName, Created) = (Rules.FileName(fileName), storedName, now);

    public bool IsImage => Path.GetExtension(FileName).ToLowerInvariant() is ".png" or ".jpg" or ".jpeg" or ".gif" or ".webp";
}
