namespace BuildingBlocks.Domain.ValueObjects;

public sealed class AuditInfo : ValueObject
{
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CreatedAt;
        yield return CreatedBy;
        yield return LastModifiedAt;
        yield return LastModifiedBy;
    }
}