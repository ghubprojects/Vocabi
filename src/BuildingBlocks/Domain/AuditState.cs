namespace BuildingBlocks.Domain;

public sealed class AuditState
{
    public DateTime CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }

    public DateTime? LastModifiedAt { get; private set; }
    public string? LastModifiedBy { get; private set; }

    public void MarkCreated(string? user)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = user;
    }

    public void MarkModified(string? user)
    {
        LastModifiedAt = DateTime.UtcNow;
        LastModifiedBy = user;
    }
}