namespace PlasticModelApp.Application.Shared.Exceptions;

/// <summary>
/// Exception class for forbidden errors in the application.
/// This class represents exceptions that occur when a user attempts to access a resource or perform an action that they do not have permission to access or perform.
/// This can include scenarios such as unauthorized access to a protected resource, attempting to perform an action that requires higher privileges, 
/// or any other situation where the user's permissions are insufficient to complete the requested operation.
/// </summary>
public sealed class ForbiddenException : AppException
{
    public ForbiddenException(string code, string message, object[] details, DateTime timestamp)
        : base(code, message, details, timestamp)
    {
    }
}
