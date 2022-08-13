using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ShareBook.Application.Common.Behaviours;
using ShareBook.Shared;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application;

public static class Extensions
{
	public static IServiceCollection AddApplication(
		this IServiceCollection services)
	{
		var assembly = Assembly.GetExecutingAssembly();
		services.AddValidation();
		
		return services
		       .AddRequestHandlers(assembly)
		       .AddAutoMapper(config => config.AddMaps(assembly))
		       .AddValidation();
	}
	
	private static IServiceCollection AddValidation(
		this IServiceCollection services)
		=> services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

	private static IServiceCollection AddRequestHandlers(
		this IServiceCollection services,
		params Assembly[] assemblies)
	{
		services.AddRequests(assemblies);
		
		services.TryDecorate(typeof (IRequestHandler<>), typeof (RequestValidationHandler<>));
		services.TryDecorate(typeof (IRequestHandler<,>), typeof (RequestValidationHandler<,>));
		services.TryDecorate(typeof (IRequestHandler<,>), typeof (LoggingHandlerDecorator<,>));
		return services;
	}
}