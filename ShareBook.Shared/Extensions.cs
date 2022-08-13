using Microsoft.Extensions.DependencyInjection;
using ShareBook.Shared.Abstractions.Requests;
using System.Reflection;

namespace ShareBook.Shared;

public static class Extensions
{
	public static IServiceCollection AddRequests(
		this IServiceCollection services,
		params Assembly[] assemblies)
		=> services
		   .AddSingleton<IRequestDispatcher, InMemoryRequestDispatcher>()
		   .Scan(s => s.FromAssemblies(assemblies)
		               .AddClasses(c =>
			               c.AssignableToAny(
				                typeof (IRequestHandler<>),
				                typeof (IRequestHandler<,>))
			                .Where(t => !t.IsAbstract && !t.IsInterface))
		               .AsImplementedInterfaces()
		               .WithScopedLifetime());
}