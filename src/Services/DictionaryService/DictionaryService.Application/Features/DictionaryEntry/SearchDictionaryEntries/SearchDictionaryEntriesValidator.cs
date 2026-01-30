using FluentValidation;

namespace DictionaryService.Application.Features.DictionaryEntry.SearchDictionaryEntries;

public sealed class SearchDictionaryEntriesQueryValidator : AbstractValidator<SearchDictionaryEntriesQuery>
{
    public SearchDictionaryEntriesQueryValidator()
    {
        RuleFor(x => x.Keyword)
            .NotEmpty()
            .WithMessage("Keyword is required.")
            .MinimumLength(2)
            .WithMessage("Keyword must be at least 2 characters.")
            .MaximumLength(100)
            .WithMessage("Keyword must not exceed 100 characters.");
    }
}
