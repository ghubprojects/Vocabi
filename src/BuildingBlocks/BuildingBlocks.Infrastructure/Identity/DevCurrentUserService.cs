using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Models;

namespace BuildingBlocks.Infrastructure.Identity;

public class DevCurrentUserService : ICurrentUser
{
    public SessionInfo? Session => new(
        Guid.Empty,
        "System",
        "example@mail.com",
        []
    );
}