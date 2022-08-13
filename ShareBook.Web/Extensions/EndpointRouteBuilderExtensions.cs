namespace ShareBook.Web.Extensions;
public static class EndpointRouteBuilderExtensions
{
	public static IEndpointRouteBuilder MapFallbackRoute(
		this IEndpointRouteBuilder routeBuilder)
	{
		routeBuilder.MapFallback(() => Results.NotFound(new
		{
			Message = "Sorry, endpoint not found!"
		}));
		
		return routeBuilder;
	}	
}