using Vocabi.Application.Services.Identity;

namespace Vocabi.Infrastructure.Services.Identity;

public class DevHttpContextCurrentUserAccessor : ICurrentUserAccessor
{
    public SessionInfo? SessionInfo
    {
        get
        {
            return new SessionInfo
            {
                UserName = "The Anh",
                UserId = "ID001",
                Email = "admin@example.email.com",
                Roles = ["Admin", "User"]
            };
        }
    }
}