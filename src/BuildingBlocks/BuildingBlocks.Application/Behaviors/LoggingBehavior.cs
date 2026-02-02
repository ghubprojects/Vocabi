using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ICurrentUser currentUser,
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = request.GetGenericTypeName();
        var userId = currentUser.Session?.UserId;

        logger.LogInformation("Handling {RequestName} for user {UserId}", requestName, userId);
        var response = await next(cancellationToken);
        logger.LogInformation("Handled {RequestName}", requestName);

        return response;
    }
}