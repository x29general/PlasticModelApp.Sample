using System;

namespace PlasticModelApp.Application.Shared.Exceptions;

/// <summary>
/// Exception class for system errors in the application.
/// This class represents exceptions that occur due to system-related issues, such as database errors, external service failures, 
/// or other unexpected conditions that are not directly related to user input or validation errors.
/// </summary>
public class AppSystemException : AppException
{
    public AppSystemException(string code, string message, object[] details, DateTime timestamp)
        : base(code, message, details, timestamp)
    {
    }
}
