namespace BuildingBlocks.Domain;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    string? CreatedBy { get; }

    DateTime? LastModifiedAt { get; }
    string? LastModifiedBy { get; }
}