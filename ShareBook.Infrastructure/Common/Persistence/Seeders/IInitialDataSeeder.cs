namespace ShareBook.Infrastructure.Common.Persistence.Seeders;

public interface IInitialDataSeeder
{
	Task SeedAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default);	
}