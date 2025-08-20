namespace Vocabi.Web.Services.Navigation;

public interface INavigationService
{
    void GoToVocabularyList();
    void GoToVocabularyCreate();
    void GoToVocabularyEdit(Guid id);
    void GoToVocabularyDetail(Guid id);
}