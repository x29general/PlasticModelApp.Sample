namespace PlasticModelApp.Application.Shared.Exceptions;

/// <summary>
/// 認可エラー（403）を表す例外。
/// </summary>
public sealed class ForbiddenException : AppException
{
    public ForbiddenException(string code, string message, object[] details, DateTime timestamp)
        : base(code, message, details, timestamp)
    {
    }
}
