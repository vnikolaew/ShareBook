using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Neo4jClient;
using Neo4jClient.Transactions;
using Newtonsoft.Json;
using ShareBook.Application.Common.Contracts;
using ShareBook.Application.Feed;
using ShareBook.Application.Follows;
using ShareBook.Application.Identity;
using ShareBook.Application.Likes;
using ShareBook.Application.Posts;
using ShareBook.Application.Profile;
using ShareBook.Domain.Common;
using ShareBook.Infrastructure.Common.Persistence.Mappings;
using ShareBook.Infrastructure.Common.Persistence.Repositories;
using ShareBook.Infrastructure.Common.Persistence.Seeders;
using ShareBook.Infrastructure.Common.Reflection;
using ShareBook.Infrastructure.Common.Security;
using ShareBook.Infrastructure.Common.Settings;
using ShareBook.Infrastructure.Feed;
using ShareBook.Infrastructure.Feed.Repositories;
using ShareBook.Infrastructure.Follows;
using ShareBook.Infrastructure.Identity.Services;
using ShareBook.Infrastructure.Identity.Settings;
using ShareBook.Infrastructure.Images.Services;
using ShareBook.Infrastructure.Images.Settings;
using ShareBook.Infrastructure.Likes;
using ShareBook.Infrastructure.Posts;
using ShareBook.Infrastructure.System;

namespace ShareBook.Infrastructure;
public static class Extensions
{
  public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
      => services
         .AddGraphData(configuration)
         .AddAuthentication(configuration)
         .AddAuthorization()
         .AddSecurity()
         .AddServices()
         .AddFactories()
         .AddSettings(configuration);

  public static IServiceCollection AddGraphData(
        this IServiceCollection services,
        IConfiguration configuration)
    => services
       .AddGraphDatabase(configuration)
       .AddGraphRepositories()
       .AddGraphRelationshipRepositories();

  public static IServiceCollection AddGraphDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
      {
        var databaseSettings = services.AddSetting<GraphDatabaseSettings>(configuration);
        
        var graphClient = new BoltGraphClient(
              new Uri(databaseSettings.BoltEndpoint),
                databaseSettings.Username,
              databaseSettings.Password)
        {
          DefaultDatabase = databaseSettings.DefaultDatabase
        };
        
        graphClient.JsonConverters
                   .AddRange(GetJsonConverters(Assembly.GetExecutingAssembly()));
        graphClient
              .ConnectAsync()
              .GetAwaiter()
              .GetResult();
        
        return services
            .AddSingleton<IBoltGraphClient>(graphClient)
            .AddSingleton<ITransactionalGraphClient>(graphClient)
            .AddSingleton<IRawGraphClient>(graphClient)
            .AddScoped<IUnitOfWork, GraphUnitOfWork>()
            .AddHostedService<GraphDatabaseInitializationService>()
            .AddJsonConverters()
            .AddDataSeeding()
            .AddGraphMappings();
      }

    private static IServiceCollection AddJsonConverters(
      this IServiceCollection services)
      => services.Scan(s =>
        s.FromExecutingAssembly()
         .AddClasses(c =>
           c.AssignableTo<JsonConverter>())
         .As<JsonConverter>()
         .WithTransientLifetime());

    private static IEnumerable<JsonConverter> GetJsonConverters(
      params Assembly[] assemblies)
      => assemblies
         .Select(a => a.GetTypes()
                       .Where(t => !t.IsInterface
                                   && !t.IsAbstract
                                   && t.IsAssignableTo(typeof (JsonConverter))))
         .SelectMany(t => t)
         .Select<Type, object>(Activator.CreateInstance)
         .Cast<JsonConverter>();

    private static IServiceCollection AddDataSeeding(
      this IServiceCollection services)
      => services.Scan(s =>
            s.FromAssemblyOf<IInitialDataSeeder>()
             .AddClasses(c =>
               c.AssignableTo<IInitialDataSeeder>())
             .AsImplementedInterfaces()
             .WithScopedLifetime());

    public static IServiceCollection AddGraphRepositories(
      this IServiceCollection services)
      => services.Scan(s =>
          s.FromAssemblies(Assembly.GetExecutingAssembly())
           .AddClasses(c =>
             c.AssignableToAny(typeof (GraphRepository<,>),
                                typeof (AuditableGraphRepository<,>))
              .Where(c => !c.IsAbstract
                          && !c.IsInterface))
             .AsImplementedInterfaces()
           .As(t => new[] { t.BaseType })
           .WithScopedLifetime());

    public static IServiceCollection AddGraphRelationshipRepositories(
      this IServiceCollection services)
      => services.Scan(s =>
        s.FromAssemblies(Assembly.GetExecutingAssembly())
         .AddClasses(c =>
           c.AssignableToAny(typeof (GraphRelationshipRepository<,,,,>))
            .Where(c => !c.IsAbstract
                        && !c.IsInterface))
         .AsImplementedInterfaces()
         .As(t => new[] { t.BaseType })
         .WithScopedLifetime());

    public static IServiceCollection AddGraphMappings(
      this IServiceCollection services)
      => services.Scan(s =>
        s.FromAssembliesOf(typeof (INodeMapping<>))
         .AddClasses(c =>
           c.AssignableTo(typeof (INodeMapping<>))
            .Where(c => !c.IsAbstract
                        && !c.IsInterface))
         .AsImplementedInterfaces()
         .WithTransientLifetime());

    public static IServiceCollection AddAuthentication(
      this IServiceCollection services,
      IConfiguration configuration)
    {
      var jwtSettings = services.AddSetting<JwtSettings>(configuration);
      
      var defaultValidationParameters = new TokenValidationParameters
      {
          ValidIssuer = jwtSettings.ValidIssuer,
          ValidAudience = jwtSettings.ValidAudience,
          IssuerSigningKey = jwtSettings.SecurityKey,
          // ValidateIssuerSigningKey = true,
          // ValidateIssuer = true,
          // ValidateAudience = true,
      };
      
      services
        .AddAuthentication(options =>
        {
          options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
          options.RequireHttpsMetadata = false;
          options.SaveToken = true;
          options.TokenValidationParameters = defaultValidationParameters;
          options.Events = new JwtBearerEvents
          {
            OnChallenge = async context =>
            {
              context.HttpContext.Response.StatusCode = 401;
              await context.HttpContext.Response.WriteAsJsonAsync(new
              {
                Error = "Sorry, but you are not authorized."
              });
            }
          };
        });
      
      return services.AddScoped<IJwtService, JwtService>();
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
      => services
         .AddTransient<IPostService, PostService>()
         .AddTransient<IIdentityService, IdentityService>()
         .AddTransient<IFollowService, FollowsService>()
         .AddTransient<ILikesService, LikesService>()
         .AddTransient<IFeedService, FeedService>()
         .AddTransient<IMediaService, MediaService>()
         .AddTransient<IFileStorageUploadService, AzureBlobService>()
         .AddTransient<IFeedRepository, FeedRepository>()
         .AddSingleton<IAccessorProvider, AccessorProvider>()
         .AddSingleton<IDateTime, DateTimeProvider>();

    public static IServiceCollection AddFactories(this IServiceCollection services)
      => services.Scan(s =>
        s.FromAssembliesOf(typeof (IFactory<>))
         .AddClasses(c =>
           c.AssignableTo(typeof (IFactory<>))
            .Where(t => !t.IsAbstract
                        && !t.IsInterface))
         .AsImplementedInterfaces()
         .WithTransientLifetime());

    public static IServiceCollection AddSecurity(this IServiceCollection services)
      => services
          .AddScoped<IPasswordHasher, PasswordHasher>()
          .AddScoped(typeof (IPasswordHasher<>),
                   typeof (PasswordHasher<>));

    public static TSetting AddSetting<TSetting>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TSetting : class, new()
    {
      var setting = new TSetting();
      
      configuration.Bind(typeof (TSetting).Name, setting);
      services.AddSingleton(setting);
      
      return setting;
    }

    public static IServiceCollection AddSettings(
        this IServiceCollection services,
        IConfiguration configuration)
    {
      services.AddSetting<AzureBlobStorageSettings>(configuration);
      return services;
    }
  }