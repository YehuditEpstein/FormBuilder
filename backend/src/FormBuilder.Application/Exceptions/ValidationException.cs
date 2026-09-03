namespace FormBuilder.Application.Exceptions;

/// <summary>Thrown when an incoming request fails business validation. Mapped to HTTP 400 by the API.</summary>
public class ValidationException : Exception
{
    public ValidationException(string message)
        : base(message)
    {
    }

    public ValidationException(IEnumerable<string> errors)
        : base(string.Join(" ", errors))
    {
        Errors = errors.ToArray();
    }

    public IReadOnlyCollection<string> Errors { get; } = Array.Empty<string>();
}
