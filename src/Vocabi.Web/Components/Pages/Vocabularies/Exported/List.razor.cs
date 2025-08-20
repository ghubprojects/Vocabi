using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.Vocabularies.GetPaginatedExportedVocabularies;

namespace Vocabi.Web.Components.Pages.Vocabularies.Exported;

public partial class List
{
    private FluentDataGrid<ExportedVocabularyDto> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };

    private IQueryable<ExportedVocabularyDto> vocabularyList = default!;
    private string searchWord = string.Empty;

    private bool isRefreshingData = false;

    private async Task RefreshItemsAsync(GridItemsProviderRequest<ExportedVocabularyDto> req)
    {
        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new GetPaginatedExportedVocabulariesQuery()
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

    private async Task HandleUnexportAsync(Guid id)
    {
        //await ExecuteAsync(async () =>
        //{
        //    var result = await Mediator.Send(new UnexportVocabularyCommand { Id = id });
        //    if (result.IsSuccess)
        //    {
        //        ToastService.ShowSuccess("Vocabulary unexported successfully.");
        //        await RefreshDataAsync();
        //    } 
        //    else
        //        ToastService.ShowError(result.ErrorMessages);
        //});
    }
}