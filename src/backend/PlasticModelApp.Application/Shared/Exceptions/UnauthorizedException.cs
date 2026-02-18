namespace PlasticModelApp.Application.Shared.Exceptions;

/// <summary>
/// 認証エラー（401）を表す例外。
/// </summary>
public sealed class UnauthorizedException : AppException
{
    public UnauthorizedException(string code, string message, object[] details, DateTime timestamp)
        : base(code, message, details, timestamp)
    {
    }
}
