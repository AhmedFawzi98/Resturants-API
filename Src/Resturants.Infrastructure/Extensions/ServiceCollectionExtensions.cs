using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resturants.Domain.Constants;
using Resturants.Domain.Entities;
using Resturants.Domain.Interfaces.Repositories;
using Resturants.Infrastructure.Authorization;
using Resturants.Infrastructure.Persistance;
using Resturants.Infrastructure.Repositories;
using Resturants.Infrastructure.Seeders;
using Microsoft.AspNetCore.Authorization;
using Resturants.Infrastructure.Authorization.Requirements.AllowedNationality;
using Resturants.Infrastructure.Constants;
using Resturants.Domain.Interfaces.Services;
using Resturants.Infrastructure.Authorization.Services;
using Microsoft.AspNetCore.Http;
using Resturants.Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;

namespace Resturants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuraion)
    {
        string connectionString = configuraion.GetConnectionString("DefaultConnection")!;
        services.AddDbContext<RestaurantDbContext>(options =>
            options.UseSqlServer(connectionString).EnableSensitiveDataLogging()
        );

        services.AddScoped<ISeeder, Seeder>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddIdentityApiEndpoints<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddClaimsPrincipalFactory<RestaurantClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<RestaurantDbContext>();

        services.ConfigureApplicationCookie(options => 
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.ExpireTimeSpan = TimeSpan.FromDays(14);
        });

        services.AddAuthorizationBuilder()
                .AddPolicy(PoliciesConstants.IsAllowedNationality, builder =>
                {
                    builder.RequireClaim(AppClaimTypesConstants.Nationality);
                    builder.AddRequirements(new NationalityRequirement(NationalityConstants.AllowedNationalities));
                });

        services.AddScoped<IAuthorizationHandler, NationalityRequirementHandler>();
        
        services.AddScoped<IRestaurantAuthoriazationService, RestaurantAuthoriazationService>();


        services.Configure<BlobStorageSettings>(configuraion.GetSection(BlobStorageSettings.BlobStorage));
        services.AddScoped<IBlobStorageService, BlobStorageService>();

    }
}
