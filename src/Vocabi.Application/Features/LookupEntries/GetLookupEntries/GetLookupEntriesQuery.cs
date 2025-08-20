using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabi.Domain.Aggregates.LookupEntries;

namespace Vocabi.Application.Features.LookupEntries.GetLookupEntries;

public class GetLookupEntriesQuery : IRequest<IReadOnlyList<LookupEntryDto>>
{
    public IEnumerable<Guid> Ids { get; init; } = [];
}

public class GetLookupEntriesQueryHandler(
    ILookupEntryRepository lookupEntryRepository,
    IMapper mapper
    ) : IRequestHandler<GetLookupEntriesQuery, IReadOnlyList<LookupEntryDto>>
{
    public async Task<IReadOnlyList<LookupEntryDto>> Handle(GetLookupEntriesQuery request, CancellationToken cancellationToken)
    {
        var query = lookupEntryRepository.DbSet
            .AsNoTracking()
            .Where(e => request.Ids.Contains(e.Id));

        return await query
            .ProjectTo<LookupEntryDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}

public class GetLookupEntriesQueryValidator : AbstractValidator<GetLookupEntriesQuery>
{
    public GetLookupEntriesQueryValidator()
    {
        RuleFor(x => x.Ids)
            .NotNull().WithMessage("Ids must not be null.")
            .Must(ids => ids.Any()).WithMessage("At least one Id is required.")
            .ForEach(idRule => idRule.NotEmpty().WithMessage("Id must not be empty."));
    }
}