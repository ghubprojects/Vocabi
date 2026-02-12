using Microsoft.AspNetCore.Components;
using WebHost.Common;

namespace WebHost.Services.Navigation;

public class NavigationService(NavigationManager nav) : INavigationService
{
    public void GoToVocabularyPendingList() => nav.NavigateTo(Routes.VocabularyPendingList);
    public void GoToVocabularyExportedList() => nav.NavigateTo(Routes.VocabularyExportedList);
    public void GoToVocabularyFailedList() => nav.NavigateTo(Routes.VocabularyFailedList);

    public void GoToVocabularyCreate() => nav.NavigateTo(Routes.VocabularyCreate);
    public void GoToVocabularyDetail(Guid id) => nav.NavigateTo($"{Routes.VocabularyDetail}/{id}");
    public void GoToVocabularyEdit(Guid id) => nav.NavigateTo($"{Routes.VocabularyEdit}/{id}");
}