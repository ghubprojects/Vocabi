using BuildingBlocks.Application.Models;

namespace BuildingBlocks.Application.Abstractions;

public interface ICurrentUser
{
    SessionInfo? Session { get; }
}