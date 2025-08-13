using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.MediaFiles;

public interface IMediaFileRepository : IRepository<MediaFile>
{
    Task AddAsync(MediaFile entity);
    Task UpdateAsync(MediaFile entity);
    Task DeleteAsync(Guid id);
}
