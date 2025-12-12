using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.Vocabularies.Commands;
using Vocabi.Application.Features.Vocabularies.Queries;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Web.Common.Enums;
using Vocabi.Web.Components.Dialogs;
using Vocabi.Web.Models.Vocabularies;

namespace Vocabi.Web.Pages.Vocabularies.Pending;

public partial class List
{
    private FluentDataGrid<VocabularyItemModel> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };

    private IQueryable<VocabularyItemModel> dataGridItems = default!;
    private IEnumerable<VocabularyItemModel> selectedItems = [];

    private string searchWord = string.Empty;

    private bool isRefreshingData = false;

    private async Task RefreshItemsAsync(GridItemsProviderRequest<VocabularyItemModel> req)
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

            dataGridItems = Mapper.Map<IReadOnlyList<VocabularyItemModel>>(result.Items).AsQueryable();
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

    private void HandleDoubleClickRow(FluentDataGridRow<VocabularyItemModel> row)
    {
        if (row.Item is not null)
            Navigation.GoToVocabularyDetail(row.Item.Id);
    }

    private async Task OpenDeleteDialogAsync(VocabularyItemModel model)
    {
        var dialogParams = new DialogParameters { Title = $"Confirm Delete" };
        var dialog = await DialogService.ShowDialogAsync<DeleteDialog>(model.Word, dialogParams);

        var dialogResult = await dialog.Result;
        if (!dialogResult.Cancelled && dialogResult.Data is not null)
        {
            await ExecuteWithLoadingAsync(async () =>
            {
                var result = await Mediator.Send(new DeleteVocabularyCommand(model.Id));
                if (result.IsFailed)
                {
                    foreach (var error in result.Errors)
                        ToastService.ShowError(error.Message);
                    return;
                }

                ToastService.ShowSuccess("Vocab deleted successfully.");
                await RefreshDataAsync();
            },
            x => loadingStates[$"{model.Id}-{RowAction.Delete}"] = x);
        }
    }

    private async Task HandleExportAsync(Guid id)
    {
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new ExportVocabularyFlashcardCommand(id));
            if (result.IsFailed)
            {
                foreach (var error in result.Errors)
                    ToastService.ShowError(error.Message);
                return;
            }

            ToastService.ShowSuccess("Vocabulary exported to Anki successfully.");
            await RefreshDataAsync();
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
            if (result.IsFailed)
            {
                foreach (var error in result.Errors)
                    ToastService.ShowError(error.Message);
                return;
            }

            ToastService.ShowSuccess("Selected vocabularies exported to Anki successfully.");
            await RefreshDataAsync();
        },
        x => isExportingMultiple = x);
    }

    #endregion
}