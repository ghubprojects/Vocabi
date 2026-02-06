namespace BuildingBlocks.Infrastructure.Persistence.Abstractions;

public interface IDatabaseSeeder
{
    Task SeedAsync();
}