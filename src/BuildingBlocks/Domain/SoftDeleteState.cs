namespace BuildingBlocks.Domain;

public sealed class SoftDeleteState
{
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }

    public void MarkDeleted(string? user)
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = user;
    }

    public void MarkRestored()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }
}