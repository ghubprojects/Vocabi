namespace BuildingBlocks.Infrastructure.Persistence.Enums;

public enum DatabaseInitMode
{
    None,
    Migrate,
    Recreate,
    RecreateAndSeed
}