using MediatR;
using Microsoft.Extensions.Logging;
using Vocabi.Application.Services.Identity;

namespace Vocabi.Application.Behaviors;

public class GlobalExceptionBehavior<TRequest, TResponse>(ILogger<TRequest> logger, ICurrentUserAccessor currentUserAccessor)
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
            var userName = currentUserAccessor.SessionInfo?.UserName;
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