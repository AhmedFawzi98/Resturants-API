using Microsoft.AspNetCore.Authorization;

namespace Resturants.Infrastructure.Authorization.Requirements.AllowedNationality;

internal class NationalityRequirementHandler : AuthorizationHandler<NationalityRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NationalityRequirement requirement)
    {
        var userNationality = context.User.Claims.FirstOrDefault(c => c.Type == "Nationality")?.Value;
        
        if (userNationality is not null && requirement.AllowedNationalities.Contains(userNationality.ToLower()))
            context.Succeed(requirement);
        else
            context.Fail();

        return Task.CompletedTask;
    }
}
