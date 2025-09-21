using Microsoft.EntityFrameworkCore;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.SeedWork;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class MediaFileRepository(ApplicationDbContext context) : IMediaFileRepository
{
    public IUnitOfWork UnitOfWork => context;

    public IQueryable<MediaFile> GetQueryableSet()
    {
        return context.Set<MediaFile>();
    }

    public async Task<MediaFile?> GetByIdAsync(Guid id)
    {
        return await context.MediaFiles
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<MediaFile>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await context.MediaFiles
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();
    }

    public async Task AddAsync(MediaFile entity)
    {
        await context.MediaFiles.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<MediaFile> entities)
    {
        await context.MediaFiles.AddRangeAsync(entities);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await context.MediaFiles.FindAsync(id);
        if (entity != null)
        {
            context.MediaFiles.Remove(entity);
        }
    }
}
