namespace FormBuilder.Application.Exceptions;

/// <summary>Thrown when a requested resource does not exist. Mapped to HTTP 404 by the API.</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }

    public static NotFoundException ForEntity(string entityName, object id) =>
        new($"{entityName} with id '{id}' was not found.");
}
