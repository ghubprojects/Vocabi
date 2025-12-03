namespace Vocabi.Web.Common;

public static class Routes
{
    // Vocabularies
    private const string Vocabulary = "/vocabularies";
    public const string VocabularyDetail = Vocabulary + "/detail";

    // Vocabulary pending
    private const string VocabularyPending = Vocabulary + "/pending";
    public const string VocabularyPendingList = VocabularyPending;
    public const string VocabularyPendingCreate = VocabularyPending + "/create";
    public const string VocabularyPendingEdit = VocabularyPending + "/edit";

    // Vocabulary exported
    private const string VocabularyExported = Vocabulary + "/exported";
    public const string VocabularyExportedList = VocabularyExported;

    // Vocabulary failed
    private const string VocabularyFailed = Vocabulary + "/failed";
    public const string VocabularyFailedList = VocabularyFailed;
}