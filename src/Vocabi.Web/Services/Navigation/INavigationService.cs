namespace Vocabi.Web.Services.Navigation;

public interface INavigationService
{
    void GoToVocabularyPendingList();
    void GoToVocabularyPendingCreate();
    void GoToVocabularyPendingEdit(Guid id);
    void GoToVocabularyPendingDetail(Guid id);
    void GoToVocabularyExportedList();
    void GoToVocabularyExportedDetail(Guid id);
    void GoToVocabularyFailedList();
    void GoToVocabularyFailedDetail(Guid id);

}