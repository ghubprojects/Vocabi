using MediatR;
using Microsoft.Extensions.Logging;
using Vocabi.Application.Services.Identity;

namespace Vocabi.Application.Behaviors;

public class GlobalExceptionBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ILogger<TRequest> _logger;

    public GlobalExceptionBehavior(ILogger<TRequest> logger, ICurrentUserAccessor currentUserAccessor)
    {
        _logger = logger;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            var userName = _currentUserAccessor.SessionInfo?.UserName;
            _logger.LogError(ex,
                "Request: {RequestName} by User: {UserName} failed. Error: {ErrorMessage}. Request Details: {@Request}",
                requestName,
                userName,
                ex.Message,
                request);
            throw;
        }
    }
}