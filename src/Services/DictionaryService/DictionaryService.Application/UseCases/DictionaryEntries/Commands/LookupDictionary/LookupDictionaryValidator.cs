using FluentValidation;

namespace DictionaryService.Application.UseCases.DictionaryEntries.Commands.LookupDictionary;

public sealed class LookupDictionaryValidator : AbstractValidator<LookupDictionaryCommand>
{
    public LookupDictionaryValidator()
    {
        RuleFor(x => x.Keyword)
            .NotEmpty()
            .WithMessage("Keyword is required.");
    }
}
