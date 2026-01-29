namespace BuildingBlocks.Application.Models;

public sealed record SessionInfo(
    Guid? UserId,
    string? UserName,
    string? Email,
    IReadOnlyCollection<string> Roles)
{
    public bool IsAuthenticated => UserId.HasValue;
}