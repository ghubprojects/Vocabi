using BuildingBlocks.Domain.ValueObjects;

namespace BuildingBlocks.Domain.Abstractions;

public interface IAuditable
{
    AuditInfo Audit { get; }
}