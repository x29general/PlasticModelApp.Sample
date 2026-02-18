using System;

namespace PlasticModelApp.Application.Shared.Exceptions;

/// <summary>
/// リソース未発見例外クラス
/// </summary>
public class NotFoundException : AppException
{
    public NotFoundException(string code, string message, object[] details, DateTime timestamp)
        : base(code, message, details, timestamp)
    {
    }
}
