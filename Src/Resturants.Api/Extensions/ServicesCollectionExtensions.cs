using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Resturants.Api.Constants;
using Resturants.Api.Middlewares;
using Resturants.Infrastructure.CustomJsonConverters;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Resturants.Api.Extensions;

public static class ServicesCollectionExtensions
{
    public static void AddPresentaion(this IServiceCollection services, IConfiguration configuraion)
    {
        #region Logger

        var loggerConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
        var connectionString = configuraion.GetConnectionString("DefaultConnection");

        var applicationInsightsConnectionString = configuraion["ApplicationInsights:InstrumentationKey"];
        services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = applicationInsightsConnectionString;
        });

        var serviceProvider = services.BuildServiceProvider();
        var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();  

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(loggerConfig)
            .WriteTo.ApplicationInsights(telemetryConfiguration,
                TelemetryConverter.Traces,restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
            .WriteTo.MSSqlServer(connectionString,
                new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    SchemaName = "logging",
                    AutoCreateSqlTable = true
                },restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
            .WriteTo.File(
                path: "wwwroot/SeriLogs/logs-.log",
                outputTemplate: "[{Level}] {Timestamp:dd-MM HH:mm:ss} || {Message}{NewLine}{Exception}",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true
            )
            .Enrich.WithProperty("ApplicationName", "Restaurants.WebApiApplication")
            .CreateLogger();
        
        services.AddSerilog();
        #endregion

        services.AddControllers()
                .AddJsonOptions(options => 
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter())
                );

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPoliciesConstants.AllowAllMethodsAndOriginsAndHeaders, policyOptions =>
            {
                policyOptions.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
           
        services.AddAuthentication();

        #region Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "Version 1",
                Title = "Restaurant Management API",
                Description = "A simple WebApi using .net core for managing restaurants data",
                Contact = new OpenApiContact
                {
                    Name = "Ahmed Fawzi",
                    Email = "ahmedfawzielarabi98@gmail.com",
                }
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                Description = "enter token:"
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
                        },
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new string[] {}
                }
            });

            options.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date",
                Example = new OpenApiString(DateTime.Today.ToString("yyyy-MM-dd"))
            });
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        #endregion

        services.AddScoped<RequestTimeLoggingMiddleware>();

        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<ForbiddenExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        
    }
}
