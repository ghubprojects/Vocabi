namespace Vocabi.Web.Services.Navigation;

public interface INavigationService
{
    void GoToList();
    void GoToCreate();
    void GoToEdit(Guid id);
    void GoToDetail(Guid id);
}