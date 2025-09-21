using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.Vocabularies.Commands;
using Vocabi.Application.Features.Vocabularies.Queries;
using Vocabi.Web.ViewModels.Vocabularies.Exported;

namespace Vocabi.Web.Components.Pages.Vocabularies.Exported;

public partial class List
{
    private FluentDataGrid<ExportedVocabularyListItemViewModel> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };

    private IQueryable<ExportedVocabularyListItemViewModel> dataGridItems = default!;
    private IEnumerable<ExportedVocabularyListItemViewModel> selectedItems = [];

    private string searchWord = string.Empty;

    private bool isRefreshingData = false;
    private bool isAnyItemSelected => selectedItems.Any();
    private bool isRemovingExportMultiple = false;

    private async Task RefreshItemsAsync(GridItemsProviderRequest<ExportedVocabularyListItemViewModel> req)
    {
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new GetPaginatedExportedVocabulariesQuery()
            {
                SearchWord = searchWord,
                PageIndex = req.StartIndex / req.Count!.Value,
                PageSize = req.Count!.Value,
            });

            dataGridItems = Mapper.Map<IReadOnlyList<ExportedVocabularyListItemViewModel>>(result.Items).AsQueryable();
            await pagination.SetTotalItemCountAsync(result.TotalItems);
        },
        x => isRefreshingData = x);
    }

    private async Task RefreshDataAsync()
    {
        await dataGrid.RefreshDataAsync(true);
        StateHasChanged();
    }

    private async Task HandleRemoveExportAsync(Guid id)
    {
        await ExecuteAsync(async () =>
        {
            var result = await Mediator.Send(new UnexportVocabularyFlashcardCommand(id));
            if (result.IsSuccess)
            {
                ToastService.ShowSuccess("Vocabulary unexported to Anki successfully.");
                await RefreshDataAsync();
            }
            else
                ToastService.ShowError(result.ErrorMessages);
        });
    }

    private async Task HandleRemoveExportMultipleAsync()
    {
        if (!isAnyItemSelected)
            return;

        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new UnexportVocabularyFlashcardsCommand(selectedItems.Select(x => x.Id)));
            if (result.IsSuccess)
            {
                ToastService.ShowSuccess("Selected vocabularies unexported to Anki successfully.");
                await RefreshDataAsync();
            }
            else
                ToastService.ShowError(result.ErrorMessages);
        },
        x => isRemovingExportMultiple = x);
    }
}