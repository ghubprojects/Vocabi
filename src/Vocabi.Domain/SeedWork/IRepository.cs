using Microsoft.EntityFrameworkCore;

namespace Vocabi.Domain.SeedWork;

public interface IRepository<T> where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
    IQueryable<T> GetQueryableSet();
}
