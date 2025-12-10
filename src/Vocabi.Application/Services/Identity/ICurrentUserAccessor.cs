namespace Vocabi.Application.Services.Identity;

/// <summary>
/// Provides information about the currently logged-in user.
/// </summary>
public interface ICurrentUserAccessor
{
    /// <summary>
    /// Gets the session info for the current user.
    /// Returns null if no user is authenticated.
    /// </summary>
    SessionInfo? SessionInfo { get; }
}