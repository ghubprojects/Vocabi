namespace BuildingBlocks.Infrastructure.Persistence.Abstractions;

public interface IDbContextInitializer
{
    Task InitializeAsync();
}