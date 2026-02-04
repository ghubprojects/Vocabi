using BuildingBlocks.Application.Abstractions;
using FluentResults;

namespace DictionaryService.Application.UseCases.DictionaryEntries.Commands.LookupDictionary;

public sealed record LookupDictionaryCommand(
    string Keyword
) : ICommand<Result>;