using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.ValueObjects;

namespace VocabularyService.Domain.Aggregates;

public sealed class Vocabulary : AggregateRoot, IAuditable, ISoftDeletable
{
    public string Word { get; private set; } = string.Empty;
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

    private Vocabulary(string word, string partOfSpeech, string pronunciation, string cloze, string definition, string translation)
    {
        Word = word;
        PartOfSpeech = partOfSpeech;
        Pronunciation = pronunciation;
        Cloze = cloze;
        Definition = definition;
        Translation = translation;
    }

    public static Vocabulary Create(string word, string partOfSpeech, string pronunciation, string cloze, string definition, string translation)
    {
        return new Vocabulary(word, partOfSpeech, pronunciation, cloze, definition, translation);
    }

    public void AddExample(string text)
    {
        _examples.Add(VocabularyExample.Create(text));
    }
}
