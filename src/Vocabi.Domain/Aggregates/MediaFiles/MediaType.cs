using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.MediaFiles;

public class MediaType : ValueObject
{
    public static readonly MediaType Unknown = new("Unknown");
    public static readonly MediaType Audio = new("Audio");
    public static readonly MediaType Image = new("Image");
    public static readonly MediaType Video = new("Video");
    public static readonly MediaType Document = new("Document");
    public static readonly MediaType Other = new("Other");

    public string Value { get; private set; }

    private MediaType(string value)
    {
        Value = value;
    }

    public static MediaType FromContentType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return Unknown;

        if (contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            return Image;

        if (contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
            return Video;

        if (contentType.StartsWith("audio/", StringComparison.OrdinalIgnoreCase))
            return Audio;

        if (contentType.StartsWith("application/pdf", StringComparison.OrdinalIgnoreCase) || contentType.StartsWith("text/", StringComparison.OrdinalIgnoreCase))
            return Document;

        return Other;
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}