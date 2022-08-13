using ShareBook.Application;
using ShareBook.Infrastructure;
using ShareBook.Web.Extensions;

namespace ShareBook.Web;
public class Startup
{
	public Startup(IConfiguration configuration)
		=> Configuration = configuration;

	private IConfiguration Configuration { get; set; }

	public void ConfigureServices(IServiceCollection services)
		=> services
				.AddApplication()
				.AddInfrastructure(Configuration)
				.AddWebComponents();

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (!env.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error")
			   .UseHsts();
		}
		else
		{
			app.UseSwagger()
			   .UseSwaggerUI();
		}
		
		app
			.UseHttpsRedirection()
			.UseRouting()
			.UseAuthentication()
			.UseAuthorization()
			.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapFallbackRoute();
			});
	}	
}