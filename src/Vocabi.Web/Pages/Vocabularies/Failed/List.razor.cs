using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.Vocabularies.Commands;
using Vocabi.Application.Features.Vocabularies.Queries;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Extensions;
using Vocabi.Web.Models.Vocabularies;

namespace Vocabi.Web.Pages.Vocabularies.Failed;

public partial class List
{
    private FluentDataGrid<VocabularyItemModel> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };

    private IQueryable<VocabularyItemModel> dataGridItems = default!;
    private IEnumerable<VocabularyItemModel> selectedItems = [];

    private string searchWord = string.Empty;

    private bool isRefreshingData = false;
    private bool isAnyItemSelected => selectedItems.Any();
    private bool isRetryingExportMultiple = false;

    private async Task RefreshItemsAsync(GridItemsProviderRequest<VocabularyItemModel> req)
    {
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new GetPagedVocabulariesQuery()
            {
                SearchWord = searchWord,
                Status = ExportStatus.Failed,
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

    private async Task HandleRetryExportAsync(Guid id)
    {
        await ExecuteAsync(async () =>
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
                ToastService.ShowError(result.GetErrorMessages());
        },
        x => isRetryingExportMultiple = x);
    }
}