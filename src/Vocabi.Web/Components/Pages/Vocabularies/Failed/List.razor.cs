using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.Vocabularies.Commands;
using Vocabi.Application.Features.Vocabularies.Queries;
using Vocabi.Web.ViewModels.Vocabularies.Failed;

namespace Vocabi.Web.Components.Pages.Vocabularies.Failed;

public partial class List
{
    private FluentDataGrid<FailedVocabularyListItemViewModel> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };

    private IQueryable<FailedVocabularyListItemViewModel> dataGridItems = default!;
    private IEnumerable<FailedVocabularyListItemViewModel> selectedItems = [];

    private string searchWord = string.Empty;

    private bool isRefreshingData = false;
    private bool isAnyItemSelected => selectedItems.Any();
    private bool isRetryingExportMultiple = false;

    private async Task RefreshItemsAsync(GridItemsProviderRequest<FailedVocabularyListItemViewModel> req)
    {
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new GetPaginatedFailedVocabulariesQuery()
            {
                SearchWord = searchWord,
                PageIndex = req.StartIndex / req.Count!.Value,
                PageSize = req.Count!.Value,
            });

            dataGridItems = Mapper.Map<IReadOnlyList<FailedVocabularyListItemViewModel>>(result.Items).AsQueryable();
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

    private async Task HandleRetryExportMultipleAsync()
    {
        if (!isAnyItemSelected)
            return;

        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new ExportVocabularyFlashcardsCommand(selectedItems.Select(x => x.Id)));
            if (result.IsSuccess)
            {
                ToastService.ShowSuccess("Selected vocabularies exported to Anki successfully.");
                await RefreshDataAsync();
            }
            else
                ToastService.ShowError(result.ErrorMessages);
        },
        x => isRetryingExportMultiple = x);
    }
}