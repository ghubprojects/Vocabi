using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Common.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationTasks = validators.Select(v => v.ValidateAsync(request, cancellationToken));
        var validationResults = await Task.WhenAll(validationTasks);

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .ToList();

        if (failures.Count > 0)
        {
            logger.LogWarning(
                "Validation errors for {RequestName}: {@Errors}",
                request.GetGenericTypeName(),
                failures.Select(e => new { e.PropertyName, e.ErrorCode }));

            throw new ValidationException(failures);
        }

        return await next(cancellationToken);
    }
}