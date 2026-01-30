using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.ValueObjects;

namespace VocabularyService.Domain.Aggregates;

public sealed class VocabularyExample : Entity, IAuditable, ISoftDeletable
{
    public string Text { get; private set; } = string.Empty;

    public AuditInfo Audit { get; private set; } = new();
    public SoftDeleteInfo SoftDelete { get; private set; } = new();

    private VocabularyExample() { }

    private VocabularyExample(string text)
    {
        Text = text;
    }

    internal static VocabularyExample Create(string text)
    {
        return new VocabularyExample(text);
    }
}