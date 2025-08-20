using Microsoft.AspNetCore.Components;
using Vocabi.Web.Common;
using Vocabi.Web.Common.Helpers;

namespace Vocabi.Web.Services.Navigation;

public class NavigationService(NavigationManager nav) : INavigationService
{
    public void GoToVocabularyList() => nav.NavigateTo(Routes.VocabularyList);
    public void GoToVocabularyCreate() => nav.NavigateTo(Routes.VocabularyCreate);
    public void GoToVocabularyEdit(Guid id) => nav.NavigateTo(Routes.VocabularyEdit.WithQuery(new { id }));
    public void GoToVocabularyDetail(Guid id) => nav.NavigateTo(Routes.VocabularyDetail.WithQuery(new { id }));
}