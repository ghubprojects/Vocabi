using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.ValueObjects;

public class Flashcard : ValueObject
{
    public string Front { get; }
    public string Back { get; }
    public IReadOnlyCollection<string> Tags { get; }

    public Flashcard(string front, string back, IEnumerable<string> tags = null)
    {
        if (string.IsNullOrWhiteSpace(front))
            throw new ArgumentException("Front cannot be empty.", nameof(front));

        if (string.IsNullOrWhiteSpace(back))
            throw new ArgumentException("Back cannot be empty.", nameof(back));

        Front = front.Trim();
        Back = back.Trim();
        Tags = tags?.ToArray() ?? Array.Empty<string>();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Front;
        yield return Back;

        // equality của Tags: bạn có thể chọn cách
        // - chỉ so sánh nội dung (không phân biệt thứ tự)
        // - hoặc giữ nguyên thứ tự
        foreach (var tag in Tags.OrderBy(t => t))
        {
            yield return tag;
        }
    }
}