using Microsoft.EntityFrameworkCore;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class MediaFileRepository(ApplicationDbContext context) : Repository<MediaFile>(context), IMediaFileRepository
{
    public async Task<MediaFile?> GetByIdAsync(Guid id)
    {
        return await DbSet
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IReadOnlyList<MediaFile>> GetByIdsAsync(IReadOnlyCollection<Guid> ids)
    {
        return await DbSet
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();
    }

    public async Task AddAsync(MediaFile entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<MediaFile> entities)
    {
        await DbSet.AddRangeAsync(entities);
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
