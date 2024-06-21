using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Resturants.Domain.Entities;
using Resturants.Infrastructure.Constants;
using System.Security.Claims;

namespace Resturants.Infrastructure.Authorization;

public class RestaurantClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    public RestaurantClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) 
        : base(userManager, roleManager, options)
    {
       
    }
    public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        ClaimsIdentity identity = await GenerateClaimsAsync(user);

        if(user.Nationality is not null)
            identity.AddClaim(new Claim(AppClaimTypesConstants.Nationality, user.Nationality));


        if (user.DateOfBirth is not null)
            identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString("yyyy-MM-dd")));
     
        return new ClaimsPrincipal(identity);
    }
}
