using BuildingBlocks.Application.Abstractions;

namespace BuildingBlocks.Domain.Abstractions;

public interface IRepository<T> where T : AggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
