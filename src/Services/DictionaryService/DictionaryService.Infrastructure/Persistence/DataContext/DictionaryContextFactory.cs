using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DictionaryService.Infrastructure.Persistence.DataContext;

public class DictionaryContextFactory : IDesignTimeDbContextFactory<DictionaryContext>
{
    public DictionaryContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DictionaryContext>();

        optionsBuilder
            .UseNpgsql("Server=localhost;Port=5432;Database=vocabi_dev;Uid=postgres;Pwd=123456;")
            .UseSnakeCaseNamingConvention();

        return new DictionaryContext(optionsBuilder.Options);
    }
}