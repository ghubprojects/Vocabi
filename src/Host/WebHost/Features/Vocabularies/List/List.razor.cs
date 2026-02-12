using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using VocabularyService.Application.UseCases.Vocabularies.GetVocabulary;
using VocabularyService.Application.UseCases.Vocabularies.SearchVocabularies;
using WebHost.Common.Helpers;
using WebHost.Components.Common.Dialogs;
using WebHost.Components.Vocabularies;
using WebHost.Services.Navigation;

namespace WebHost.Features.Vocabularies.List;

public partial class List
{
	[SupplyParameterFromQuery]
	public string? Keyword { get; set; }

	[SupplyParameterFromQuery]
	public int? PageIndex { get; set; }

	[SupplyParameterFromQuery]
	public int? PageSize { get; set; }

	[Parameter] public string StatusText { get; set; } = default!;

    [Inject] protected VocabularyNavigation Navigation { get; private set; } = default!;
    [Inject] protected IDialogService DialogService { get; private set; } = default!;
    [Inject] protected IToastService ToastService { get; private set; } = default!;
	[Inject] protected IMediator Mediator { get; private set; } = default!;
    [Inject] protected IMapper Mapper { get; private set; } = default!;
    [Inject] protected IActionExecutor ActionExecutor { get; private set; } = default!;

    public VocabularyStatus Status { get; set; }

    private bool isRefreshing;

    private bool isDeletingMultiple;
    private bool isExportingMultiple;
    private bool isRetryingExportMultiple;
    private bool isRemovingExportMultiple;

    private FluentDataGrid<SearchItemViewModel> dataGrid = default!;
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };

    private IEnumerable<SearchItemViewModel> dataGridItems = [];

    private IEnumerable<SearchItemViewModel> selectedItems => dataGridItems.Where(x => x.IsSelected);

	

	protected override void OnParametersSet()
    {
        if (!Enum.TryParse<VocabularyStatus>(StatusText, ignoreCase: true, out var parsed))
        {
            //Navigation.NavigateTo("/not-found");
            return;
        }

        Status = parsed;
    }

    private async Task RefreshItemsAsync(GridItemsProviderRequest<SearchItemViewModel> req)
    {
        //var exportStatus = Status switch
        //{
        //    VocabularyStatus.Pending => ExportStatus.Pending,
        //    VocabularyStatus.Exported => ExportStatus.Completed,
        //    VocabularyStatus.Failed => ExportStatus.Failed,
        //    _ => ExportStatus.Pending
        //};
        var pageIndex = req.StartIndex / req.Count!.Value;
        var pageSize = req.Count.Value;

        //await ActionExecutor.ExecuteAsync(
        //    () => Mediator.Send(new SearchVocabulariesQuery(Keyword, pageIndex, pageSize)),
        //    isLoading => isRefreshing = isLoading,
        //    async result =>
        //    {
        //        dataGridItems = Mapper.Map<IReadOnlyList<SearchItemViewModel>>(result.Items);
        //        await pagination.SetTotalItemCountAsync(result.TotalItems);
        //    }
        //);

        isRefreshing = true;
        try
        {
            var result = await Mediator.Send(new SearchVocabulariesQuery(Keyword, pageIndex, pageSize));

            dataGridItems = Mapper.Map<IReadOnlyList<SearchItemViewModel>>(result.Vocabularies.Items);
            await pagination.SetTotalItemCountAsync(result.Vocabularies.TotalItems);
        }
        finally
        {
            isRefreshing = false;
        }
        StateHasChanged();
    }

    private Task RefreshDataAsync() => dataGrid.RefreshDataAsync(true);

    private void HandleDoubleClickRow(FluentDataGridRow<SearchItemViewModel> row)
    {
        if (row.Item is not null)
            Navigation.ToDetail(row.Item.Id);
    }

    private async Task OpenCreateDialogAsync()
    {
        //var dialogParams = new DialogParameters
        //{
        //    Title = $"Create Vocabulary",
        //    Width = "1000px",
        //    PreventDismissOnOverlayClick = true
        //};
        //var dialog = await DialogService.ShowDialogAsync<VocabularyDetailDialog>(dialogParams);
        //var dialogResult = await dialog.Result;

        //if (!dialogResult.Cancelled && dialogResult.Data is not null)
        //    await RefreshDataAsync();
    }

    private async Task OpenEditDialogAsync(SearchItemViewModel viewModel)
    {
        //var dialogParams = new DialogParameters
        //{
        //    Title = $"Edit Vocabulary",
        //    Width = "1000px",
        //    PreventDismissOnOverlayClick = true
        //};
        //var dialog = await DialogService.ShowDialogAsync<VocabularyDetailDialog>(viewModel.Headword, dialogParams);
        //var dialogResult = await dialog.Result;

        //if (!dialogResult.Cancelled && dialogResult.Data is not null)
        //    await RefreshDataAsync();
    }

    private async Task OpenDeleteDialogAsync(SearchItemViewModel viewModel)
    {
        //var dialogParams = new DialogParameters
        //{
        //    Title = $"Delete Vocabulary"
        //};
        //var dialog = await DialogService.ShowDialogAsync<DeleteDialog>(viewModel.Word, dialogParams);
        //var dialogResult = await dialog.Result;

        //if (!dialogResult.Cancelled && dialogResult.Data is not null)
        //{
        //    await ActionExecutor.ExecuteAsync(
        //        () => Mediator.Send(new DeleteVocabularyCommand(viewModel.Id)),
        //        isLoading => viewModel.IsDeleting = isLoading,
        //        RefreshDataAsync
        //    );
        //}
    }

    private async Task HandleExportAsync(SearchItemViewModel viewModel)
    {
        //await ActionExecutor.ExecuteAsync(
        //    () => Mediator.Send(new ExportVocabularyFlashcardCommand(viewModel.Id)),
        //    isLoading => viewModel.IsExporting = isLoading,
        //    RefreshDataAsync
        //);
    }

    private async Task HandleRemoveExportAsync(SearchItemViewModel viewModel)
    {
        //await ActionExecutor.ExecuteAsync(
        //   () => Mediator.Send(new UnexportVocabularyFlashcardCommand(viewModel.Id)),
        //   isLoading => viewModel.IsRemovingExport = isLoading,
        //   RefreshDataAsync
        //);
    }

    private async Task HandleRetryExportAsync(SearchItemViewModel viewModel)
    {
       // await ActionExecutor.ExecuteAsync(
       //   () => Mediator.Send(new ExportVocabularyFlashcardCommand(viewModel.Id)),
       //   isLoading => viewModel.IsRetryingExport = isLoading,
       //   RefreshDataAsync
       //);
    }

    private async Task HandleExportMultipleAsync()
    {
        //await ActionExecutor.ExecuteAsync(
        //   () => Mediator.Send(new ExportVocabularyFlashcardsCommand(selectedItems.Select(x => x.Id))),
        //   isLoading => isExportingMultiple = isLoading,
        //   RefreshDataAsync
        //);
    }

    private async Task HandleRemoveExportMultipleAsync()
    {
        //await ActionExecutor.ExecuteAsync(
        //   () => Mediator.Send(new UnexportVocabularyFlashcardsCommand(selectedItems.Select(x => x.Id))),
        //   isLoading => isRemovingExportMultiple = isLoading,
        //   RefreshDataAsync
        //);
    }

    private async Task HandleRetryExportMultipleAsync()
    {
        //await ActionExecutor.ExecuteAsync(
        //   () => Mediator.Send(new ExportVocabularyFlashcardsCommand(selectedItems.Select(x => x.Id))),
        //   isLoading => isRetryingExportMultiple = isLoading,
        //   RefreshDataAsync
        //);
    }

    public enum VocabularyStatus
    {
        Pending,
        Exported,
        Failed
    }
}