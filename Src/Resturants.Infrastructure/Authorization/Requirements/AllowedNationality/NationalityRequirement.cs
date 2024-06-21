using Microsoft.AspNetCore.Authorization;

namespace Resturants.Infrastructure.Authorization.Requirements.AllowedNationality;

public class NationalityRequirement : IAuthorizationRequirement
{
    public string[] AllowedNationalities { get; private set; }

    public NationalityRequirement(string[] allowedNationalities)
    {
        AllowedNationalities = allowedNationalities;
    }
}