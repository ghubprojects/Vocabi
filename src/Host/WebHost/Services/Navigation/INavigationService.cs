namespace WebHost.Services.Navigation;

public interface INavigationService
{
    void GoToVocabularyPendingList();
    void GoToVocabularyExportedList();
    void GoToVocabularyFailedList();

    void GoToVocabularyCreate();
    void GoToVocabularyDetail(Guid id);
    void GoToVocabularyEdit(Guid id);
}