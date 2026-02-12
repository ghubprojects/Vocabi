//using AutoMapper;
//using MediatR;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Forms;
//using Microsoft.FluentUI.AspNetCore.Components;
//using Vocabi.Application.Features.MediaFiles.DTOs;
//using Vocabi.Application.Features.Vocabularies.DTOs;
//using Vocabi.Application.Features.Vocabularies.Queries;
//using WebHost.Common.Helpers;
//using WebHost.Services.Navigation;

//namespace WebHost.Features.Vocabularies.Detail;

//public partial class Detail
//{
//    [Parameter] public Guid Id { get; set; }

//     [Inject] protected IDialogService DialogService { get; private set; } = default!;
//    [Inject] protected INavigationService Navigation { get; private set; } = default!;
//    [Inject] protected IMediator Mediator { get; private set; } = default!;
//    [Inject] protected IMapper Mapper { get; private set; } = default!;
//    [Inject] protected IActionExecutor ActionExecutor { get; private set; } = default!;

//    private EditContext editContext = default!;

//    private VocabularyDto vocabularyForm = new();
//    private MediaFileDto audioFile = new();
//    private MediaFileDto imageFile = new();

//    protected override async Task OnInitializedAsync()
//    {
//        editContext = new EditContext(vocabularyForm);
//        //vocabularyForm = await Mediator.Send(new GetVocabularyQuery(Id));
//        audioFile = vocabularyForm.AudioFile;
//        imageFile = vocabularyForm.ImageFile;
//    }
//}