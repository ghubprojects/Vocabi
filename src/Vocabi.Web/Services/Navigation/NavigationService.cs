using Microsoft.AspNetCore.Components;
using Vocabi.Web.Common;

namespace Vocabi.Web.Services.Navigation;

public class NavigationService(NavigationManager nav) : INavigationService
{
    public void GoToVocabularyPendingList() => nav.NavigateTo(Routes.VocabularyPendingList);
    public void GoToVocabularyPendingCreate() => nav.NavigateTo(Routes.VocabularyPendingCreate);
    public void GoToVocabularyPendingEdit(Guid id) => nav.NavigateTo($"{Routes.VocabularyPendingEdit}/{id}");
    public void GoToVocabularyExportedList() => nav.NavigateTo(Routes.VocabularyExportedList);
    public void GoToVocabularyFailedList() => nav.NavigateTo(Routes.VocabularyFailedList);
    public void GoToVocabularyDetail(Guid id) => nav.NavigateTo($"{Routes.VocabularyDetail}/{id}");
}