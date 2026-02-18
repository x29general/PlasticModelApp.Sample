using System;

namespace PlasticModelApp.Application.Shared.Exceptions
{
    /// <summary>
    /// アプリケーション全体の基底例外クラス
    /// </summary>
    public abstract class AppException : Exception
    {
        public string Code { get; }
        public override string Message { get; }
        public object[] Details { get; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;

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