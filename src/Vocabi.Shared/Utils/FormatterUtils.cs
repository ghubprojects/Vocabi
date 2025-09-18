namespace Vocabi.Shared.Utils;

public static class FormatterUtils
{
    public static string WrapWithSlashes(string text)
        => string.IsNullOrWhiteSpace(text) ? string.Empty : $"/{text.Trim('/')}/";

    public static string TrimSlashes(string text)
        => string.IsNullOrWhiteSpace(text) ? string.Empty : text.Trim('/').Trim();

    public static string BuildImageTag(string filename)
        => string.IsNullOrWhiteSpace(filename) ? string.Empty : $"<img src=\"{filename}\" />";

    public static string BuildSoundTag(string filename)
        => string.IsNullOrWhiteSpace(filename) ? string.Empty : $"[sound:{filename}]";
}
