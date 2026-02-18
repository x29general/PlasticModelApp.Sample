using System;
using System.Collections.Generic;

namespace PlasticModelApp.Application.Shared.Exceptions;

/// <summary>
/// バリデーションエラー用の例外クラス
/// </summary>
public class ValidationException : AppException
{
    public ValidationException(string code, string message, object[] errors, DateTime timestamp)
        : base(code, message, errors.ToArray(), timestamp)
    {
    }
}
