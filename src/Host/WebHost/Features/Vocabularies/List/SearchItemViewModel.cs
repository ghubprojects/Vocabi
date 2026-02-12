namespace WebHost.Features.Vocabularies.List;

public sealed class SearchItemViewModel
{
    public Guid Id { get; init; }
    public string Headword { get; init; } = string.Empty;
    public string PartOfSpeech { get; init; } = string.Empty;
    public string Translation { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }

    public bool IsSelected { get; set; }

    public RowAction CurrentAction { get; set; } = RowAction.None;
    public bool IsExporting => CurrentAction is RowAction.Export;
    public bool IsRemoving => CurrentAction is RowAction.Remove;
    public bool IsBusy => CurrentAction is not RowAction.None;
}
