using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.Vocabularies.Commands;
using Vocabi.Application.Features.Vocabularies.Queries;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Web.Common.Enums;
using Vocabi.Web.Components.Dialogs;
using Vocabi.Web.ViewModels.Vocabularies;

namespace Vocabi.Web.Components.Pages.Vocabularies.Pending;

public partial class List
{
    private FluentDataGrid<VocabularyListItemViewModel> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };

    private IQueryable<VocabularyListItemViewModel> dataGridItems = default!;
    private IEnumerable<VocabularyListItemViewModel> selectedItems = [];

    private string searchWord = string.Empty;

    private bool isRefreshingData = false;

    private async Task RefreshItemsAsync(GridItemsProviderRequest<VocabularyListItemViewModel> req)
    {
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new GetPagedVocabulariesQuery()
            {
                SearchWord = searchWord,
                Status = ExportStatus.Pending,
                PageIndex = req.StartIndex / req.Count!.Value,
                PageSize = req.Count!.Value,
            });

            dataGridItems = Mapper.Map<IReadOnlyList<VocabularyListItemViewModel>>(result.Items).AsQueryable();
            await pagination.SetTotalItemCountAsync(result.TotalItems);
        },
        x => isRefreshingData = x);
    }

    private async Task RefreshDataAsync()
    {
        await dataGrid.RefreshDataAsync(true);
        StateHasChanged();
    }

    #region Single Row Actions
    private readonly Dictionary<string, bool> loadingStates = [];

    private bool IsLoading(Guid id, RowAction action)
       => loadingStates.TryGetValue($"{id}-{action}", out var isLoading) && isLoading;

    private void HandleDoubleClickRow(FluentDataGridRow<VocabularyListItemViewModel> row)
    {
        if (row.Item is not null)
            Navigation.GoToVocabularyDetail(row.Item.Id);
    }

    private async Task OpenDeleteDialogAsync(VocabularyListItemViewModel model)
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
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new ExportVocabularyFlashcardCommand(id));
            if (result.IsSuccess)
            {
                ToastService.ShowSuccess("Vocabulary exported to Anki successfully.");
                await RefreshDataAsync();
            }
            else
                ToastService.ShowError(result.ErrorMessages);
        },
        x => loadingStates[$"{id}-{RowAction.Export}"] = x);
    }
    #endregion

    #region Multiple Row Actions
    private bool isAnyItemSelected => selectedItems.Any();
    private bool isDeletingMultiple = false;
    private bool isExportingMultiple = false;

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
    #endregion
}