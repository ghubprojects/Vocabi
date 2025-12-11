using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Vocabi.Application.Features.MediaFiles.DTOs;
using Vocabi.Application.Features.Vocabularies.DTOs;
using Vocabi.Application.Features.Vocabularies.Queries;

namespace Vocabi.Web.Pages.Vocabularies;

public partial class Detail
{
    [Parameter] public Guid Id { get; set; }

    private EditContext editContext = default!;

    private VocabularyDto vocabularyForm = new();
    private MediaFileDto audioFile = new();
    private MediaFileDto imageFile = new();

    protected override async Task OnInitializedAsync()
    {
        editContext = new EditContext(vocabularyForm);
        vocabularyForm = await Mediator.Send(new GetVocabularyQuery(Id));
        audioFile = vocabularyForm.AudioFile;
        imageFile = vocabularyForm.ImageFile;
    }
}