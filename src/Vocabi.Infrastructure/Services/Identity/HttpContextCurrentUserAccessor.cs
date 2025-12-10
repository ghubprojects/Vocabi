using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Vocabi.Application.Services.Identity;

namespace Vocabi.Infrastructure.Services.Identity;

public class HttpContextCurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public SessionInfo? SessionInfo
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity?.IsAuthenticated == true)
                return null;

            return new SessionInfo
            {
                UserName = user.Identity.Name,
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = user.FindFirst(ClaimTypes.Email)?.Value,
                Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray()
            };
        }
    }
}