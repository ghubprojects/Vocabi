using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.ValueObjects;
using VocabularyService.Domain.Aggregates.Rules;

namespace VocabularyService.Domain.Aggregates;

public sealed class Vocabulary : AggregateRoot, IAuditable, ISoftDeletable
{
    public string Headword { get; private set; } = string.Empty;
    public string PartOfSpeech { get; private set; } = string.Empty;
    public string Pronunciation { get; private set; } = string.Empty;
    public string Cloze { get; private set; } = string.Empty;
    public string Definition { get; private set; } = string.Empty;
    public string Translation { get; private set; } = string.Empty;

    private readonly List<VocabularyExample> _examples = [];
    public IReadOnlyCollection<VocabularyExample> Examples => _examples.AsReadOnly();

    public AuditInfo Audit { get; private set; } = new();
    public SoftDeleteInfo SoftDelete { get; private set; } = new();

    private Vocabulary() { }

    private Vocabulary(string headword, string partOfSpeech, string pronunciation, string cloze, string definition, string translation)
    {
        Headword = headword;
        PartOfSpeech = partOfSpeech;
        Pronunciation = pronunciation;
        Cloze = cloze;
        Definition = definition;
        Translation = translation;
    }

    public static Vocabulary Create(string headword, string partOfSpeech, string pronunciation, string cloze, string definition, string translation)
    {
        return new Vocabulary(headword, partOfSpeech, pronunciation, cloze, definition, translation);
    }

    public void AddExample(string text)
    {
        CheckRule(new CannotModifyDeletedVocabularyRule(SoftDelete.IsDeleted));

        _examples.Add(VocabularyExample.Create(text));
    }
}
