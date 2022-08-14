using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Settings;

namespace ShareBook.Infrastructure.Common.Persistence.Seeders;
public class GraphDatabaseInitializationService : BackgroundService
{
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly GraphDatabaseSettings _settings;
	private readonly ILogger<GraphDatabaseInitializationService> _logger;

	public GraphDatabaseInitializationService(
		IServiceScopeFactory scopeFactory,
		GraphDatabaseSettings settings,
		ILogger<GraphDatabaseInitializationService> logger)
	{
		_scopeFactory = scopeFactory;
		_settings = settings;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(
		CancellationToken stoppingToken)
	{
		await using var scope = _scopeFactory.CreateAsyncScope();
		try
		{
			if (_settings.UseDataSeeding && await DatabaseIsEmpty(scope.ServiceProvider))
			{
				await SeedData(scope.ServiceProvider, stoppingToken);
			}

		}
		catch (Exception e)
		{
			_logger.LogInformation("Error occured: {Message}", e.Message);
			throw;
		}
	}
	
	private static async Task<bool> DatabaseIsEmpty(IServiceProvider provider)
	{
		var graphClient = provider.GetRequiredService<IRawGraphClient>();
		return await graphClient
		             .Cypher
		             .Match("(a)")
		             .Return<int>("count(a)")
		             .GetResult() == 0;
	}
	
	private async Task SeedData(
		IServiceProvider provider,
		CancellationToken cancellationToken)
	{ 
		var seeders = provider.GetServices<IInitialDataSeeder>();
		_logger.LogInformation("Starting to seed initial data using: [{Seeders}]", string.Join(", ", seeders.Select(s => s.GetType().Name)));
	  
		foreach (var seeder in seeders)
		{
			await seeder.SeedAsync(provider, cancellationToken);
		}
	  
		_logger.LogInformation("Finished seeding initial data ...");
	}
}