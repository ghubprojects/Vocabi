using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocabi.Application.Behaviors;

// ExceptionMappingBehavior.cs
public class ExceptionMappingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger _logger;
    public ExceptionMappingBehavior(ILogger<ExceptionMappingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        try { return await next(); }
        catch (DomainException dex)
        {
            _logger.LogWarning(dex, "Domain exception for {Request}", typeof(TRequest).Name);
            throw; // Or map to an Application-specific result type
        }
        catch (ValidationException vex)
        {
            _logger.LogInformation(vex, "Validation failed");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in {Request}", typeof(TRequest).Name);
            throw; // bubble up to top-level middleware
        }
    }
}
