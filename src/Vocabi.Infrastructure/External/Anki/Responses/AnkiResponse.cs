namespace Vocabi.Infrastructure.External.Anki.Responses;

public sealed record AnkiResponse<T>(
    T? Result, 
    string? Error
);