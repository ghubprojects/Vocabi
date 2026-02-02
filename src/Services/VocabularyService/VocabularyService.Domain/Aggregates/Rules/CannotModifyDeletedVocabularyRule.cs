using BuildingBlocks.Domain.Abstractions;

namespace VocabularyService.Domain.Aggregates.Rules;

public sealed class CannotModifyDeletedVocabularyRule : IBusinessRule
{
    private readonly bool _isDeleted;

    internal CannotModifyDeletedVocabularyRule(bool isDeleted)
    {
        _isDeleted = isDeleted;
    }

    public bool IsBroken() => _isDeleted;

    public string Message => "Deleted vocabulary cannot be modified.";
}