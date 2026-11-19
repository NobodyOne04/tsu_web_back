namespace Backend5.Core;

/// <summary>Shared checks for user-entered text.</summary>
internal static class Text
{
    public const int MaxName = 200;
    public const int MaxPhone = 20;

    public static string Required(string? value, string field, int max = MaxName)
    {
        var trimmed = value?.Trim() ?? "";
        if (trimmed.Length == 0) throw new DomainException($"{field} is required.");
        if (trimmed.Length > max) throw new DomainException($"{field} must be at most {max} characters.");
        return trimmed;
    }

    public static string? Optional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    /// <summary>"a, b ,, c" → ["a", "b", "c"]; each must fit a phone column.</summary>
    public static IReadOnlyList<string> PhoneList(string? text)
    {
        var numbers = (text ?? "").Split(',').Select(p => p.Trim()).Where(p => p.Length > 0).ToList();
        foreach (var n in numbers.Where(n => n.Length > MaxPhone))
            throw new DomainException($"Phone \"{n}\" is longer than {MaxPhone} characters.");
        return numbers;
    }
}
