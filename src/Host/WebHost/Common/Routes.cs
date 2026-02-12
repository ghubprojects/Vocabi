namespace WebHost.Common;

public static class Routes
{
    private const string Vocabulary = "/vocabularies";

    public const string VocabularyPendingList = Vocabulary + "/pending";
    public const string VocabularyExportedList = Vocabulary + "/exported";
    public const string VocabularyFailedList = Vocabulary + "/failed";

    public const string VocabularyCreate = Vocabulary + "/create";
    public const string VocabularyDetail = Vocabulary + "/detail";
    public const string VocabularyEdit = Vocabulary + "/edit";
}