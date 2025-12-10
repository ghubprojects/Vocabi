namespace Vocabi.Application.Services.Identity;

/// <summary>
/// Represents information about the current session / user.
/// </summary>
public class SessionInfo
{
    /// <summary>
    /// Username of the current user
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// User Id (optional)
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Email of the current user (optional)
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Roles of the current user (optional)
    /// </summary>
    public string[]? Roles { get; set; }
}