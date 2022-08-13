namespace ShareBook.Web.Extensions;

public static class WebApplicationBuilderExtensions
{
	public static WebApplication UseStartup<TStartup>(
		this WebApplicationBuilder builder)
	{
		const string configureServices = "ConfigureServices";
		const string configure = "Configure";
		
		var startup = (TStartup) Activator.CreateInstance(typeof (TStartup), builder.Configuration);
		if (startup == null)
			throw new ArgumentNullException("startup", "Could not create TStartup class!");

		var configureServicesMethod = typeof (TStartup).GetMethod(configureServices)
		                              ?? throw new ArgumentNullException(nameof(configureServices),
			                              "Could not find ConfigureServices method!");

		configureServicesMethod.Invoke(startup, new[] {builder.Services});
		var app = builder.Build();

		var configureMethod = typeof (TStartup).GetMethod(configure)
		                      ?? throw new ArgumentNullException(nameof(configure),
			                      "Could not find Configure method!");

		configureMethod.Invoke(startup, new object[] {app, app.Environment});
		
		return app;
	}

	public static WebApplicationBuilder ConfigureKestrelHttps(
		this WebApplicationBuilder builder,
		int httpPort,
		int httpsPort)
	{
		if (builder.Environment.IsProduction())
		{
			return builder;
		}
		
		builder.WebHost.ConfigureKestrel(options =>
		{
			options.ListenLocalhost(httpPort);
			options.ListenLocalhost(httpsPort, listenOptions =>
			{
				listenOptions.UseHttps();
			});
		});
		
		return builder;
	}	
}