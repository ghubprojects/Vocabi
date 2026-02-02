using BuildingBlocks.Domain.Abstractions;

namespace BuildingBlocks.Domain;

public abstract record DomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}