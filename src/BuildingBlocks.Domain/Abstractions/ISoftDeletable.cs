using BuildingBlocks.Domain.ValueObjects;

namespace BuildingBlocks.Domain.Abstractions;

public interface ISoftDeletable
{
    SoftDeleteInfo SoftDelete { get; }
}