using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.Vocabularies.Commands;
using Vocabi.Application.Features.Vocabularies.Queries;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Web.Common.Helpers;
using Vocabi.Web.Components.Common.Dialogs;
using Vocabi.Web.Components.Vocabularies;
using Vocabi.Web.Services.Navigation;
using Vocabi.Web.ViewModels.Vocabularies;

namespace Vocabi.Web.Pages.Vocabularies;

public partial class List
{
    [Parameter] public string StatusText { get; set; } = default!;

    [Inject] protected IDialogService DialogService { get; private set; } = default!;
    [Inject] protected INavigationService Navigation { get; private set; } = default!;
    [Inject] protected IMediator Mediator { get; private set; } = default!;
    [Inject] protected IMapper Mapper { get; private set; } = default!;
    [Inject] protected IActionExecutor ActionExecutor { get; private set; } = default!;

    public VocabularyStatus Status { get; set; }

    private bool isRefreshing;
    private bool isDeletingMultiple;
    private bool isExportingMultiple;
    private bool isRetryingExportMultiple;
    private bool isRemovingExportMultiple;

    private FluentDataGrid<VocabularyDetailViewModel> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };

    private IEnumerable<VocabularyDetailViewModel> dataGridItems = [];

    private IEnumerable<VocabularyDetailViewModel> selectedItems => dataGridItems.Where(x => x.IsSelected);

    private string searchWord = string.Empty;

    protected override void OnParametersSet()
    {
        if (!Enum.TryParse<VocabularyStatus>(StatusText, ignoreCase: true, out var parsed))
        {
            //Navigation.NavigateTo("/not-found");
            return;
        }

        Status = parsed;
    }

    private async Task RefreshItemsAsync(GridItemsProviderRequest<VocabularyDetailViewModel> req)
    {
        var exportStatus = Status switch
        {
            VocabularyStatus.Pending => ExportStatus.Pending,
            VocabularyStatus.Exported => ExportStatus.Completed,
            VocabularyStatus.Failed => ExportStatus.Failed,
            _ => ExportStatus.Pending
        };
        var pageIndex = req.StartIndex / req.Count!.Value;
        var pageSize = req.Count.Value;

        await ActionExecutor.ExecuteAsync(
            () => Mediator.Send(new GetPagedVocabulariesQuery(searchWord, exportStatus, pageIndex, pageSize)),
            isLoading => isRefreshing = isLoading,
            async result =>
            {
                dataGridItems = Mapper.Map<IReadOnlyList<VocabularyDetailViewModel>>(result.Items);
                await pagination.SetTotalItemCountAsync(result.TotalItems);
            }
        );
        StateHasChanged();
    }

    private Task RefreshDataAsync() => dataGrid.RefreshDataAsync(true);

    private void HandleDoubleClickRow(FluentDataGridRow<VocabularyDetailViewModel> row)
    {
        if (row.Item is not null)
            Navigation.GoToVocabularyDetail(row.Item.Id);
    }

    private async Task OpenCreateDialogAsync()
    {
        var dialogParams = new DialogParameters
        {
            Title = $"Create Vocabulary",
            Width = "1000px",
            PreventDismissOnOverlayClick = true
        };
        var dialog = await DialogService.ShowDialogAsync<VocabularyDetailDialog>(dialogParams);
        var dialogResult = await dialog.Result;

        if (!dialogResult.Cancelled && dialogResult.Data is not null)
            await RefreshDataAsync();
    }

    private async Task OpenEditDialogAsync(VocabularyDetailViewModel viewModel)
    {
        var dialogParams = new DialogParameters
        {
            Title = $"Edit Vocabulary",
            Width = "1000px",
            PreventDismissOnOverlayClick = true
        };
        var dialog = await DialogService.ShowDialogAsync<VocabularyDetailDialog>(viewModel.Word, dialogParams);
        var dialogResult = await dialog.Result;

        if (!dialogResult.Cancelled && dialogResult.Data is not null)
            await RefreshDataAsync();
    }

    private async Task OpenDeleteDialogAsync(VocabularyDetailViewModel viewModel)
    {
        var dialogParams = new DialogParameters
        {
            Title = $"Delete Vocabulary"
        };
        var dialog = await DialogService.ShowDialogAsync<DeleteDialog>(viewModel.Word, dialogParams);
        var dialogResult = await dialog.Result;

        if (!dialogResult.Cancelled && dialogResult.Data is not null)
        {
            await ActionExecutor.ExecuteAsync(
                () => Mediator.Send(new DeleteVocabularyCommand(viewModel.Id)),
                isLoading => viewModel.IsDeleting = isLoading,
                RefreshDataAsync
            );
        }
    }

    private async Task HandleExportAsync(VocabularyDetailViewModel viewModel)
    {
        await ActionExecutor.ExecuteAsync(
            () => Mediator.Send(new ExportVocabularyFlashcardCommand(viewModel.Id)),
            isLoading => viewModel.IsExporting = isLoading,
            RefreshDataAsync
        );
    }

    private async Task HandleRemoveExportAsync(VocabularyDetailViewModel viewModel)
    {
        await ActionExecutor.ExecuteAsync(
           () => Mediator.Send(new UnexportVocabularyFlashcardCommand(viewModel.Id)),
           isLoading => viewModel.IsRemovingExport = isLoading,
           RefreshDataAsync
        );
    }

    private async Task HandleRetryExportAsync(VocabularyDetailViewModel viewModel)
    {
        await ActionExecutor.ExecuteAsync(
          () => Mediator.Send(new ExportVocabularyFlashcardCommand(viewModel.Id)),
          isLoading => viewModel.IsRetryingExport = isLoading,
          RefreshDataAsync
       );
    }

    private async Task HandleExportMultipleAsync()
    {
        await ActionExecutor.ExecuteAsync(
           () => Mediator.Send(new ExportVocabularyFlashcardsCommand(selectedItems.Select(x => x.Id))),
           isLoading => isExportingMultiple = isLoading,
           RefreshDataAsync
        );
    }

    private async Task HandleRemoveExportMultipleAsync()
    {
        await ActionExecutor.ExecuteAsync(
           () => Mediator.Send(new UnexportVocabularyFlashcardsCommand(selectedItems.Select(x => x.Id))),
           isLoading => isRemovingExportMultiple = isLoading,
           RefreshDataAsync
        );
    }

    private async Task HandleRetryExportMultipleAsync()
    {
        await ActionExecutor.ExecuteAsync(
           () => Mediator.Send(new ExportVocabularyFlashcardsCommand(selectedItems.Select(x => x.Id))),
           isLoading => isRetryingExportMultiple = isLoading,
           RefreshDataAsync
        );
    }

    public enum VocabularyStatus
    {
        Pending,
        Exported,
        Failed
    }
}