namespace BuildingBlocks.Infrastructure.Configuration;

public interface IOptionsSection
{
    static abstract string SectionName { get; }
}