using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Vocabi.Application.Common.Configurations;
using Vocabi.Application.Services.Identity;
using Vocabi.Application.Services.Interfaces;

namespace Vocabi.Application.Behaviors;

public class PerformanceBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IPerformanceRedactor _redactor;
    private readonly PerformanceSettings _options;
    private static readonly ActivitySource ActivitySource = new("Application.Performance");

    public PerformanceBehaviour(
        ILogger<PerformanceBehaviour<TRequest, TResponse>> logger,
        ICurrentUserAccessor currentUserAccessor,
        IPerformanceRedactor redactor,
        IOptions<PerformanceSettings> options)
    {
        _logger = logger;
        _currentUserAccessor = currentUserAccessor;
        _redactor = redactor;
        _options = options.Value;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        using var activity = _options.EnableTracing
            ? ActivitySource.StartActivity(typeof(TRequest).Name)
            : null;

        var stopwatch = Stopwatch.StartNew();

        var response = await next().ConfigureAwait(false);

        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > _options.ThresholdMilliseconds)
        {
            var requestName = typeof(TRequest).Name;
            var userName = _currentUserAccessor.SessionInfo?.UserName;
            var safeRequest = _redactor.Redact(request);

            _logger.LogWarning(
                "Long-running request detected: {RequestName} ({ElapsedMilliseconds}ms) {@Request} by {UserName}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                safeRequest,
                userName);
        }

        return response;
    }
}