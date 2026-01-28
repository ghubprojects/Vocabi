using BuildingBlocks.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Application.Behaviors;

public class PerformanceBehaviour<TRequest, TResponse>(
    ICurrentUser currentUser,
    ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private static readonly TimeSpan Threshold = TimeSpan.FromMilliseconds(500);

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await next(cancellationToken).ConfigureAwait(false);

        stopwatch.Stop();

        if (stopwatch.Elapsed > Threshold)
        {
            var requestName = typeof(TRequest).Name;
            var userName = currentUser.Session?.UserName;

            logger.LogWarning(
                "Request {RequestName} exceeded performance threshold ({ThresholdMs}ms). " +
                "Elapsed: {ElapsedMs}ms. User: {UserName}. Payload: {@Request}",
                requestName,
                Threshold.TotalMilliseconds,
                stopwatch.ElapsedMilliseconds,
                userName ?? "Anonymous",
                request);
        }

        return response;
    }
}