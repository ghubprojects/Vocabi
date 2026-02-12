using BuildingBlocks.Application.Abstractions;
using FluentResults;

namespace DictionaryService.Application.UseCases.DictionaryEntries.LookupDictionary;

public sealed record LookupDictionaryCommand(
    string Keyword
) : ICommand<Result>;