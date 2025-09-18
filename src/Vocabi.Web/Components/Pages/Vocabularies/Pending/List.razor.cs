using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.Vocabularies.ExportVocabularyFlashcard;
using Vocabi.Application.Features.Vocabularies.ExportVocabularyFlashcards;
using Vocabi.Application.Features.Vocabularies.GetPaginatedPendingVocabularies;
using Vocabi.Web.Components.Dialogs;
using Vocabi.Web.ViewModels;

namespace Vocabi.Web.Components.Pages.Vocabularies.Pending;

public partial class List
{
    private FluentDataGrid<PendingVocabularyListItemViewModel> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };

    private IQueryable<PendingVocabularyListItemViewModel> dataGridItems = default!;
    private IEnumerable<PendingVocabularyListItemViewModel> selectedItems = [];

    private string searchWord = string.Empty;

    private bool isRefreshingData = false;
    private bool isAnyItemSelected => selectedItems.Any();
    private bool isDeletingMultiple = false;
    private bool isExportingMultiple = false;

    private async Task RefreshItemsAsync(GridItemsProviderRequest<PendingVocabularyListItemViewModel> req)
    {
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new GetPaginatedPendingVocabulariesQuery()
            {
                SearchWord = searchWord,
                PageIndex = req.StartIndex / req.Count!.Value,
                PageSize = req.Count!.Value,
            });

            dataGridItems = Mapper.Map<IReadOnlyList<PendingVocabularyListItemViewModel>>(result.Items).AsQueryable();
            await pagination.SetTotalItemCountAsync(result.TotalItems);
        },
        x => isRefreshingData = x);
    }

    private async Task RefreshDataAsync()
    {
        await dataGrid.RefreshDataAsync(true);
        StateHasChanged();
    }

    private void HandleDoubleClickRow(FluentDataGridRow<PendingVocabularyListItemViewModel> row)
    {
        if (row.Item is not null)
            Navigation.GoToVocabularyPendingDetail(row.Item.Id);
    }

    private async Task OpenDeleteDialogAsync(PendingVocabularyListItemViewModel model)
    {
        var dialog = await DialogService.ShowDialogAsync<DeleteDialog>(model.Word, new DialogParameters
        {
            Title = $"Confirm Delete",
            Width = "480px",
            Height = "240px",
        });

        var result = await dialog.Result;
        if (!result.Cancelled && result.Data is not null)
        {
            try
            {
                //await VocabService.DeleteVocabAsync(model);
                ToastService.ShowSuccess("Vocab deleted successfully.");
                await RefreshDataAsync();
            }
            catch (Exception e)
            {
                ToastService.ShowError(e.Message);
            }
        }
    }

    private async Task HandleExportAsync(Guid id)
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

    private async Task HandleExportMultipleAsync()
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
        x => isExportingMultiple = x);
    }
}