using BuildingBlocks.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Application.Behaviors;

public class PerformanceBehavior<TRequest, TResponse>(
    ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private const int ThresholdMs = 500;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next(cancellationToken);
        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > ThresholdMs)
        {
            logger.LogWarning(
                "Long-running request {RequestName}. Elapsed: {ElapsedMs}ms (Threshold: {ThresholdMs}ms)",
                request.GetGenericTypeName(),
                stopwatch.ElapsedMilliseconds,
                ThresholdMs);
        }

        return response;
    }
}