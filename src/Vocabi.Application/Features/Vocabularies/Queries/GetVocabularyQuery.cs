using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabi.Application.Features.MediaFiles.DTOs;
using Vocabi.Application.Features.Vocabularies.DTOs;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.Queries;

public record GetVocabularyQuery(Guid VocabularyId) : IRequest<VocabularyDto>;

public class GetVocabularyQueryHandler(
    IVocabularyRepository vocabularyRepository,
    IMediaFileRepository mediaFileRepository,
    IMapper mapper
    ) : IRequestHandler<GetVocabularyQuery, VocabularyDto>
{
    public async Task<VocabularyDto> Handle(GetVocabularyQuery request, CancellationToken cancellationToken)
    {
        var vocabulary = await vocabularyRepository
            .GetQueryableSet()
            .Include(x => x.Flashcard)
            .Include(x => x.MediaFiles)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.VocabularyId);

        var mediaFiles = await mediaFileRepository
            .GetQueryableSet()
            .AsNoTracking()
            .Where(x => vocabulary != null && vocabulary.MediaFiles.Select(mf => mf.MediaFileId).Contains(x.Id))
            .ToListAsync(cancellationToken);

        var dto = mapper.Map<VocabularyDto>(vocabulary);
        dto.AudioFile = mapper.Map<MediaFileDto>(mediaFiles[0]);
        dto.ImageFile = mapper.Map<MediaFileDto>(mediaFiles[1]);

        return dto;
    }
}