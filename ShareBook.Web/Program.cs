using ShareBook.Web.Extensions;

namespace ShareBook.Web;

public class Program
{
	public static void Main(string[] args)
		=> CreateDefaultBuilder(args)
		   .UseStartup<Startup>()
		   .Run();
	private static WebApplicationBuilder CreateDefaultBuilder(string[] args)
		=> WebApplication
				.CreateBuilder(args)
				.ConfigureKestrelHttps(5064, 7064);
}