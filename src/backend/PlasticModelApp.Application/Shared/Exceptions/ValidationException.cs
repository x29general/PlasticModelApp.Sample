using System;
using System.Collections.Generic;

namespace PlasticModelApp.Application.Shared.Exceptions;

/// <summary>
/// Exception class for validation errors in the application.
/// This class represents exceptions that occur when user input fails to meet the defined validation rules or constraints.
/// This can include scenarios such as missing required fields, invalid data formats, values that are out of range, 
/// or any other situation where the input data does not conform to the expected criteria defined by the application's validation logic.
/// </summary>
public class ValidationException : AppException
{
    public ValidationException(string code, string message, object[] errors, DateTime timestamp)
        : base(code, message, errors.ToArray(), timestamp)
    {
    }
}
