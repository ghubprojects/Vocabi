using Microsoft.AspNetCore.Components;
using Vocabi.Web.Common;
using Vocabi.Web.Common.Helpers;

namespace Vocabi.Web.Services.Navigation;

public class NavigationService(NavigationManager nav) : INavigationService
{
    public void GoToList() => nav.NavigateTo(Routes.VocabularyList);
    public void GoToCreate() => nav.NavigateTo(Routes.VocabularyCreate);
    public void GoToEdit(Guid id) => nav.NavigateTo(Routes.VocabularyEdit.WithQuery(new { id }));
    public void GoToDetail(Guid id) => nav.NavigateTo(Routes.VocabularyDetail.WithQuery(new { id }));
}