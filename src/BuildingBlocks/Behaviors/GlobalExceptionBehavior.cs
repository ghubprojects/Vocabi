using BuildingBlocks.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Application.Behaviors;

public class GlobalExceptionBehavior<TRequest, TResponse>(
    ICurrentUser currentUser, 
    ILogger<GlobalExceptionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            var userName = currentUser.Session?.UserName;

            logger.LogError(ex,
                "Request: {RequestName} by User: {UserName} failed. Error: {ErrorMessage}. Request Details: {@Request}",
                requestName,
                userName,
                ex.Message,
                request);

            throw;
        }
    }
}