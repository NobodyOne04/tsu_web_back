namespace Backend5.Core;

/// <summary>A rule of the domain was broken; the message is shown to the user.</summary>
public class DomainException(string message) : Exception(message);

/// <summary>The requested thing doesn't exist.</summary>
public class NotFoundException(string what, int id) : Exception($"{what} {id} not found");
