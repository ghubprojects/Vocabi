using MediatR;

namespace Vocabi.Domain.SeedWork;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}