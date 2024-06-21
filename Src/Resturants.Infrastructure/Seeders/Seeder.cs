using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resturants.Domain.Constants;
using Resturants.Domain.Entities;
using Resturants.Infrastructure.Persistance;

namespace Resturants.Infrastructure.Seeders;

internal class Seeder : ISeeder
{
    private readonly RestaurantDbContext dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    private ApplicationUser admin = new ApplicationUser()
    {
        UserName = "admin@test.com",
        NormalizedUserName = "admin@test.com".ToUpper(),
        Email = "admin@test.com",
        NormalizedEmail = "admin@test.com".ToUpper(),
        EmailConfirmed = true,
        SecurityStamp = Guid.NewGuid().ToString(),
        ConcurrencyStamp = Guid.NewGuid().ToString(),
    };
    private ApplicationUser owner = new ApplicationUser()
    {
        UserName = "owner@test.com",
        NormalizedUserName = "owner@test.com".ToUpper(),
        Email = "owner@test.com",
        NormalizedEmail = "owner@test.com".ToUpper(),
        EmailConfirmed = true,
        SecurityStamp = Guid.NewGuid().ToString(),
        ConcurrencyStamp = Guid.NewGuid().ToString(),
    };
    private ApplicationUser user = new ApplicationUser()
    {
        UserName = "user@test.com",
        NormalizedUserName = "user@test.com".ToUpper(),
        Email = "user@test.com",
        NormalizedEmail = "user@test.com".ToUpper(),
        EmailConfirmed = true,
        SecurityStamp = Guid.NewGuid().ToString(),
        ConcurrencyStamp = Guid.NewGuid().ToString(),
    };

    public Seeder(RestaurantDbContext dbcontext, UserManager<ApplicationUser> userManager)
    {
        dbContext = dbcontext;
        _userManager = userManager;
    }

    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {
            if(!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                await dbContext.Roles.AddRangeAsync(roles);
                await dbContext.SaveChangesAsync();
            }
            if(!dbContext.Users.Any())
            {
                var resultAdmin = await _userManager.CreateAsync(admin, "adminAdmin@12345@");
                var resultOwner = await _userManager.CreateAsync(owner, "ownerOwner@12345@");
                var resultUser = await _userManager.CreateAsync(user, "userUser@12345@");

                if (resultAdmin.Succeeded && resultOwner.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, RolesConstants.Admin);                
                    await _userManager.AddToRoleAsync(owner, RolesConstants.Owner);                
                    await _userManager.AddToRoleAsync(user, RolesConstants.User);                
                }
            }
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                await dbContext.Restaurants.AddRangeAsync(restaurants);
                await dbContext.SaveChangesAsync();
            }
        }
    }
    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles = new List<IdentityRole>
        {
            CreateRole(RolesConstants.Owner),
            CreateRole(RolesConstants.Admin),
            CreateRole(RolesConstants.User)
        };

        return roles;
    }
    private IdentityRole CreateRole(string roleName)
    {
        var role = new IdentityRole(roleName);
        role.NormalizedName = roleName.ToUpper();
        role.ConcurrencyStamp = Guid.NewGuid().ToString();

        return role;
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant> restaurants = [
            new Restaurant
            {
                Owner = owner,
                Name = "KFC",
                Category = "Fast Food",
                Description =
                    "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                Dishes =
                [
                    new()
                    {
                        Name = "Nashville Hot Chicken",
                        Description = "Nashville Hot Chicken (10 pcs.)",
                        Price = 10.30M,
                    },

                    new()
                    {
                        Name = "Chicken Nuggets",
                        Description = "Chicken Nuggets (5 pcs.)",
                        Price = 5.30M,
                    },
                ],
                Address = new()
                {
                    City = "London",
                    Street = "Cork St 5",
                    PostalCode = "WC2N 5DU"
                }
            },
            new Restaurant
            {
                Owner = owner,
                Name = "McDonald",
                Category = "Fast Food",
                Description =
                    "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                ContactEmail = "contact@mcdonald.com",
                HasDelivery = true,
                Address = new Address()
                {
                    City = "London",
                    Street = "Boots 193",
                    PostalCode = "W1F 8SR"
                }
            }
        ];

        return restaurants;
    }
}
