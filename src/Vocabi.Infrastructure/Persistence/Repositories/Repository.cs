using Microsoft.EntityFrameworkCore;
using Vocabi.Domain.SeedWork;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IUnitOfWork UnitOfWork => _context;

    public DbSet<T> DbSet => _context.Set<T>();
}
