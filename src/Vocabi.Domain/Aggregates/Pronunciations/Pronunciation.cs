#nullable disable

using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.Pronunciations;

public class Pronunciation : Entity, IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Word { get; private set; }
    public string Ipa { get; private set; }

    private Pronunciation() { }

    private Pronunciation(string word, string ipa)
    {
        Word = word;
        Ipa = ipa;
    }

    public static Pronunciation CreateNew(string word, string ipa)
    {
        return new Pronunciation(word, ipa);
    }
}
