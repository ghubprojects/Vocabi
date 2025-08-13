using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class MediaFileRepository: Repository<MediaFile>, IMediaFileRepository
{
    public MediaFileRepository(ApplicationDbContext context) : base(context) { }

    public async Task AddAsync(MediaFile entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<MediaFile> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public async Task UpdateAsync(MediaFile entity)
    {
        DbSet.Update(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }
}
