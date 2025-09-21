using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabi.Application.Features.MediaFiles.DTOs;
using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Features.MediaFiles.Commands;

public class GetMediaFilesQuery : IRequest<IReadOnlyList<MediaFileDto>>
{
    public IEnumerable<Guid> Ids { get; init; } = [];
}

public class GetMediaFilesQueryHandler(
    IMediaFileRepository mediaFileRepository,
    IMapper mapper
    ) : IRequestHandler<GetMediaFilesQuery, IReadOnlyList<MediaFileDto>>
{
    public async Task<IReadOnlyList<MediaFileDto>> Handle(GetMediaFilesQuery request, CancellationToken cancellationToken)
    {
        var query = mediaFileRepository
            .GetQueryableSet()
            .AsNoTracking()
            .Where(e => request.Ids.Contains(e.Id));

        return await query
            .ProjectTo<MediaFileDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}

public class GetMediaFilesQueryValidator : AbstractValidator<GetMediaFilesQuery>
{
    public GetMediaFilesQueryValidator()
    {
        RuleFor(x => x.Ids)
            .NotNull().WithMessage("Ids must not be null.")
            .Must(ids => ids.Any()).WithMessage("At least one Id is required.")
            .ForEach(idRule => idRule.NotEmpty().WithMessage("Id must not be empty."));
    }
}
