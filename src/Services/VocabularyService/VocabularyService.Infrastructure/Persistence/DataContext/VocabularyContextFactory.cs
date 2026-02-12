using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VocabularyService.Infrastructure.Persistence.DataContext;

public class VocabularyContextFactory : IDesignTimeDbContextFactory<VocabularyContext>
{
    public VocabularyContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<VocabularyContext>();

        optionsBuilder
            .UseNpgsql("Server=localhost;Port=5432;Database=vocabi_dev;Uid=postgres;Pwd=123456;")
            .UseSnakeCaseNamingConvention();

        return new VocabularyContext(optionsBuilder.Options);
    }
}