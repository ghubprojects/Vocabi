using Microsoft.EntityFrameworkCore;

namespace Vocabi.Domain.SeedWork;

public interface IRepository<T> where T : class
{
    IUnitOfWork UnitOfWork { get; }
    DbSet<T> DbSet { get; }
}
