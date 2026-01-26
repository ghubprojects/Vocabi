namespace BuildingBlocks.Domain;

public interface IEntity
{
}

public interface IEntity<out TId>
{
    TId Id { get; }
}