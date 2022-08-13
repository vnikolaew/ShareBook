using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ShareBook.Application.Common.Contracts;
using ShareBook.Web.PipelineFeatures;
using ShareBook.Web.PipelineFeatures.Filters;
using ShareBook.Web.Services;

namespace ShareBook.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebServices(
      this IServiceCollection services)
      => services
         .AddHttpContextAccessor()
         .AddScoped<ICurrentUser, CurrentUserService>()
         .AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

    public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services)
      => services.AddSwaggerGen(options =>
      {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Description = "Standard Authorization header using the Bearer scheme(\"bearer {token}\")",
          Scheme = "Bearer",
          BearerFormat = "JWT",
          In = ParameterLocation.Header,
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey
        });
        
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
            }, Array.Empty<string>()}
        });
      });

    public static IServiceCollection AddWebComponents(this IServiceCollection services)
    {
      services
        .AddWebServices()
        .AddConfiguredSwagger()
        .AddEndpointsApiExplorer()
        .Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
        .AddControllers(options =>
        {
          options.SuppressOutputFormatterBuffering = false;
          options.Filters.Insert(0, new ValidationExceptionFilter());
          options.Filters.Insert(0, new GlobalExceptionFilter());
        });
      
      return services;
    }
}	