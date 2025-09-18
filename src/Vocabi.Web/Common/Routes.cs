namespace Vocabi.Web.Common;

public static class Routes
{
    // Vocabularies
    private const string Vocabulary = "/vocabularies";

    // Vocabulary pending
    private const string VocabularyPending = Vocabulary + "/pending";
    public const string VocabularyPendingList = VocabularyPending;
    public const string VocabularyPendingCreate = VocabularyPending + "/create";
    public const string VocabularyPendingEdit = VocabularyPending + "/edit";
    public const string VocabularyPendingDetail = VocabularyPending + "/details";

    // Vocabulary exported
    private const string VocabularyExported = Vocabulary + "/exported";
    public const string VocabularyExportedList = VocabularyExported;
    public const string VocabularyExportedDetail = VocabularyExported + "/details";

    // Vocabulary failed
    private const string VocabularyFailed = Vocabulary + "/failed";
    public const string VocabularyFailedList = VocabularyFailed;
    public const string VocabularyFailedDetail = VocabularyFailed + "/details";
}