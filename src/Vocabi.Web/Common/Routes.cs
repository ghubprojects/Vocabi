namespace Vocabi.Web.Common;

public static class Routes
{
    // Vocabularies
    private const string Vocabulary = "/vocabularies";
    private const string VocabularyPending = Vocabulary + "/pending";
    private const string VocabularyExported = Vocabulary + "/exported";

    public const string VocabularyPendingList = VocabularyPending;
    public const string VocabularyPendingCreate = VocabularyPending + "/create";
    public const string VocabularyPendingEdit = VocabularyPending + "/edit";
    public const string VocabularyPendingDetail = VocabularyPending + "/details";

    public const string VocabularyExportedList = VocabularyExported;
    public const string VocabularyExportedDetail = VocabularyExported + "/details";
}