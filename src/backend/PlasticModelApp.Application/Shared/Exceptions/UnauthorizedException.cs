namespace PlasticModelApp.Application.Shared.Exceptions;

/// <summary>
/// Exception class for unauthorized errors in the application.
/// This class represents exceptions that occur when a user attempts to access a resource or perform an action that they are not authenticated to access or perform.
/// This can include scenarios such as accessing a protected resource without providing valid authentication credentials,
/// or any other situation where the user is not authenticated and therefore cannot access the requested resource or perform the requested action.
/// </summary>
public sealed class UnauthorizedException : AppException
{
    public UnauthorizedException(string code, string message, object[] details, DateTime timestamp)
        : base(code, message, details, timestamp)
    {
    }
}
