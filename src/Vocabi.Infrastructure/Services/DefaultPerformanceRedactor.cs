using Vocabi.Application.Services.Interfaces;

namespace Vocabi.Infrastructure.Services;

public class DefaultPerformanceRedactor : IPerformanceRedactor
{
    public object Redact(object request)
    {
        // Implement redaction logic if needed (mask email, phone, etc.)
        // For now simply return request type name:
        return new { Type = request.GetType().Name };
    }
}