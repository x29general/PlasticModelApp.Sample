using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using ValidationException = PlasticModelApp.Application.Shared.Exceptions.ValidationException;
using PlasticModelApp.Application.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace PlasticModelApp.Application.Shared;

/// <summary>
/// ValidationBehavior は、MediatR パイプラインの一部としてリクエストのバリデーションを行います。
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    /// <summary>
    /// バリデーションを実行し、リクエストが有効であることを確認します。
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = results.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count > 0)
            {
                // フィールド別にエラーをグループ化
                var errorsByField = failures
                    .GroupBy(f => f.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(f => f.ErrorMessage).ToArray()
                    );

                // バリデーションエラーの詳細ログ
                foreach (var error in errorsByField)
                {
                    foreach (var message in error.Value)
                    {
                        _logger.LogWarning("バリデーションエラー: {FieldName} - {ErrorMessage}, requestType={RequestType}", 
                            error.Key, message, typeof(TRequest).Name);
                    }
                }

                throw new ValidationException(
                    "E-400-001",
                    "不正なリクエスト:リクエストに無効な値が含まれるか、必須パラメータが欠落しています。",
                    errorsByField.Select(kvp => new { Field = kvp.Key, Messages = kvp.Value }).ToArray(),
                    DateTime.UtcNow);
            }
        }

        return await next();
    }
}
