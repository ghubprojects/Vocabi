using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.MediaFiles;

public interface IMediaFileRepository : IRepository<MediaFile>
{
    Task<MediaFile?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<MediaFile>> GetByIdsAsync(IReadOnlyCollection<Guid> ids);
    Task AddAsync(MediaFile entity);
    Task AddRangeAsync(IEnumerable<MediaFile> entities);
}
