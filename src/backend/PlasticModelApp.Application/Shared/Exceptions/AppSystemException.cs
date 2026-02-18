using System;

namespace PlasticModelApp.Application.Shared.Exceptions;

/// <summary>
/// システムエラー用の例外クラス
/// </summary>
public class AppSystemException : AppException
{
    public AppSystemException(string code, string message, object[] details, DateTime timestamp)
        : base(code, message, details, timestamp)
    {
    }
}
