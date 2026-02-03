using BuildingBlocks.Application.Abstractions;
using FluentResults;

namespace DictionaryService.Application.Features.DictionaryEntry.Commands.LookupDictionary;

public sealed record LookupDictionaryCommand(
    string Keyword
) : ICommand<Result>;