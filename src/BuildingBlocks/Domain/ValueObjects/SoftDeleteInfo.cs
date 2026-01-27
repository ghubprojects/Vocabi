
namespace BuildingBlocks.Domain.ValueObjects;

public sealed class SoftDeleteInfo : ValueObject
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return IsDeleted;
        yield return DeletedAt;
        yield return DeletedBy;
    }
}