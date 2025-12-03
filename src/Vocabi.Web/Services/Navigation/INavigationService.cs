namespace Vocabi.Web.Services.Navigation;

public interface INavigationService
{
    void GoToVocabularyPendingList();
    void GoToVocabularyPendingCreate();
    void GoToVocabularyPendingEdit(Guid id);
    void GoToVocabularyExportedList();
    void GoToVocabularyFailedList();
    void GoToVocabularyDetail(Guid id);
}