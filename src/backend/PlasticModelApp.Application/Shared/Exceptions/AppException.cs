using System;

namespace PlasticModelApp.Application.Shared.Exceptions
{
    /// <summary>
    /// Exception class for application-specific errors. 
    /// This class serves as the base class for all custom exceptions in the application, 
    /// such as validation errors, not found errors, forbidden errors, and system errors. 
    /// It includes properties for an error code, a message, details about the error, and a timestamp indicating when the error occurred.
    /// </summary>
    public abstract class AppException : Exception
    {
        /// <summary>
        /// A string representing the specific error code associated with this exception.
        /// This code can be used to identify the type of error that occurred and can be helpful for debugging and error handling purposes.
        /// It is set through the constructor when the exception is created.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// A string containing a human-readable message that describes the error.
        /// This message is intended to provide more context about the error and can be used for logging or displaying error information to the user.
        /// It is set through the constructor when the exception is created and overrides the base Exception.Message property.
        /// </summary>
        public override string Message { get; }

        /// <summary>
        /// An array of objects that provides additional details about the error.
        /// This can include any relevant information that may help in understanding the error, such as validation errors, stack traces, or other contextual data.
        /// It is set through the constructor when the exception is created and can be used for logging or debugging purposes.
        /// </summary>
        public object[] Details { get; }

        /// <summary>
        /// A DateTime property that indicates when the exception was created.
        /// This timestamp is set to the current UTC time when the exception is instantiated, providing a record of when the error occurred.
        /// This can be useful for logging and debugging purposes, allowing developers to track when specific errors happen in the application.
        /// It is set through the constructor when the exception is created and is read-only.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Protected constructor for the AppException class.
        /// This constructor is intended to be called by derived classes when they create specific types of exceptions
        /// </summary>
        /// <param name="code">The specific error code associated with this exception.</param>
        /// <param name="message">A human-readable message that describes the error.</param>
        /// <param name="details">An array of objects that provides additional details about the error.</param>
        /// <param name="timestamp">A DateTime indicating when the error occurred.</param>
        protected AppException(string code, string message, object[] details, DateTime timestamp)
            : base(message)
        {
            Code = code;
            Message = message;
            Details = details;
            Timestamp = timestamp;
        }
    }
} 