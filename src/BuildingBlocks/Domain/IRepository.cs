namespace BuildingBlocks.Domain;

public interface IRepository<T> where T : AggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
