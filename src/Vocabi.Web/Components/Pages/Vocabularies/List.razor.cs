using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Vocabi.Application.Features.Vocabularies.ExportVocabularies;
using Vocabi.Application.Features.Vocabularies.GetPaginatedVocabularies;
using Vocabi.Web.Components.Dialogs;

namespace Vocabi.Web.Components.Pages.Vocabularies;

public partial class List
{
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    private FluentDataGrid<VocabularyDto> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };

    private IQueryable<VocabularyDto> vocabularyList = default!;
    private string searchWord = string.Empty;

    private bool isRefreshingData = false;
    private bool isExportingToAnki = false;

    private async Task RefreshItemsAsync(GridItemsProviderRequest<VocabularyDto> req)
    {
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new GetPaginatedVocabulariesQuery()
            {
                SearchWord = searchWord,
                PageIndex = req.StartIndex / req.Count!.Value,
                PageSize = req.Count!.Value,
            });
            vocabularyList = result.Items.AsQueryable();
            await pagination.SetTotalItemCountAsync(result.TotalItems);
        }, x => isRefreshingData = x);
    }

    private async Task RefreshDataAsync()
    {
        await dataGrid.RefreshDataAsync(true);
        StateHasChanged();
    }

    private async Task ExportToAnkiAsync()
    {
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new ExportVocabulariesCommand
            {
                //SearchWord = searchWord,
                //PageIndex = req.StartIndex / req.Count!.Value,
                //PageSize = req.Count!.Value,
            });
        }, x => isExportingToAnki = x);
    }

    private async Task OpenDeleteDialogAsync(VocabularyDto model)
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
}
