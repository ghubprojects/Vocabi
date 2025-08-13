using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Vocabi.Application.Features.Vocabularies.Models;
using Vocabi.Application.Features.Vocabularies.Queries;

namespace Vocabi.Web.Components.Pages;

public partial class VocabList
{
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    private FluentDataGrid<VocabularyModel> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 12 };

    private IQueryable<VocabularyModel> vocabularies = default!;
    private string searchWord = string.Empty;

    private async Task RefreshItemsAsync(GridItemsProviderRequest<VocabularyModel> req)
    {
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new GetPaginatedVocabEntriesQuery()
            {
                SearchWord = searchWord,
                PageIndex = req.StartIndex / req.Count!.Value,
                PageSize = req.Count!.Value,
            });
            vocabularies = result.Items.AsQueryable();
            await pagination.SetTotalItemCountAsync(result.TotalItems);
        });
    }

    private async Task RefreshDataAsync()
    {
        await dataGrid.RefreshDataAsync(true);
        StateHasChanged();
    }

    //private async Task ExportAsync()
    //{
    //    try
    //    {
    //        var searchViewModel = ViewModel.Search.Clone();
    //        searchViewModel.StartIndex = 0;
    //        searchViewModel.Count = null;
    //        var bytes = await VocabService.ExportVocabsToCsvAsync(searchViewModel.ToSearchCriteria());
    //        var fileName = $"vocabs_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
    //        await DownloadFile(fileName, bytes, "text/csv");
    //    }
    //    catch (Exception e)
    //    {
    //        ToastService.ShowError(e.Message);
    //        return;
    //    }
    //}

    //private async Task DownloadFile(string fileName, byte[] content, string contentType)
    //{
    //    await JS.InvokeVoidAsync("downloadFile", fileName, content, contentType);
    //}

    //private async Task OpenDeleteDialogAsync(VocabEntryDto model)
    //{
    //    var dialog = await DialogService.ShowDialogAsync<DeleteDialog>(model.Word, new DialogParameters
    //    {
    //        Title = $"Confirm Delete",
    //        Width = "480px",
    //        Height = "240px",
    //    });

    //    var result = await dialog.Result;
    //    if (!result.Cancelled && result.Data is not null)
    //    {
    //        try
    //        {
    //            await VocabService.DeleteVocabAsync(model);
    //            ToastService.ShowSuccess("Vocab deleted successfully.");
    //            await RefreshDataAsync();
    //        }
    //        catch (Exception e)
    //        {
    //            ToastService.ShowError(e.Message);
    //        }
    //    }
    //}
}
