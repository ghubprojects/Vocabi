using Microsoft.AspNetCore.Components;
using Vocabi.Web.Common;
using Vocabi.Web.Common.Extensions;

namespace Vocabi.Web.Services.Navigation;

public class NavigationService(NavigationManager nav) : INavigationService
{
    public void GoToVocabularyPendingList() => nav.NavigateTo(Routes.VocabularyPendingList);
    public void GoToVocabularyPendingCreate() => nav.NavigateTo(Routes.VocabularyPendingCreate);
    public void GoToVocabularyPendingEdit(Guid id) => nav.NavigateTo(Routes.VocabularyPendingEdit.WithQuery(new { id }));
    public void GoToVocabularyPendingDetail(Guid id) => nav.NavigateTo(Routes.VocabularyPendingDetail.WithQuery(new { id }));
    public void GoToVocabularyExportedList() => nav.NavigateTo(Routes.VocabularyExportedList);
    public void GoToVocabularyExportedDetail(Guid id) => nav.NavigateTo(Routes.VocabularyExportedDetail.WithQuery(new { id }));
}