using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.Vocabularies.ExportVocabularyFlashcard;
using Vocabi.Application.Features.Vocabularies.GetPaginatedFailedVocabularies;

namespace Vocabi.Web.Components.Pages.Vocabularies.Failed;

public partial class List
{
    private FluentDataGrid<FailedVocabularyDto> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };

    private IQueryable<FailedVocabularyDto> vocabularyList = default!;
    private string searchWord = string.Empty;

    private bool isRefreshingData = false;

    private async Task RefreshItemsAsync(GridItemsProviderRequest<FailedVocabularyDto> req)
    {
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new GetPaginatedFailedVocabulariesQuery()
            {
                SearchWord = searchWord,
                PageIndex = req.StartIndex / req.Count!.Value,
                PageSize = req.Count!.Value,
            });
            vocabularyList = result.Items.AsQueryable();
            await pagination.SetTotalItemCountAsync(result.TotalItems);
        },
        x => isRefreshingData = x);
    }

    private async Task RefreshDataAsync()
    {
        await dataGrid.RefreshDataAsync(true);
        StateHasChanged();
    }

    private async Task HandleRetryExportAsync(Guid id)
    {
        await ExecuteAsync(async () =>
        {
            var result = await Mediator.Send(new ExportVocabularyFlashcardCommand(id));
            if (result.IsSuccess)
            {
                ToastService.ShowSuccess("Vocabulary exported to Anki successfully.");
                await RefreshDataAsync();
            }
            else
                ToastService.ShowError(result.ErrorMessages);
        });
    }
}