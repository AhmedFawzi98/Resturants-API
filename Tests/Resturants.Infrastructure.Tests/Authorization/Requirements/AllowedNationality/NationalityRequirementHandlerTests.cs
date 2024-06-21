using Microsoft.AspNetCore.Authorization;
using Resturants.Domain.Constants;
using Resturants.Infrastructure.Authorization.Requirements.AllowedNationality;
using Resturants.Infrastructure.Constants;
using System.Security.Claims;

namespace Resturants.Infrastructure.Tests.Authorization.Requirements.AllowedNationality;
    
[TestFixture]
public class NationalityRequirementHandlerTests
{
    private NationalityRequirementHandler _cut;

    [SetUp]
    public void SetUp()
    {
        _cut = new NationalityRequirementHandler();
    }



    [TestCase("egyptian")]
    [TestCase("italian")]
    [TestCase("indian")]
    public async Task HandleRequirementAsync_IfAllowedNationality_ShouldSucceed(string nationality)
    {
        var context = CreateAuthorizationContext(nationality);

        await _cut.HandleAsync(context);


        Assert.That(context.HasSucceeded, Is.True);
    }

    [TestCase("saudi")]
    [TestCase("american")]
    [TestCase("someOtherNationality")]
    public async Task HandleRequirementAsync_IfNotAllowedNationality_ShouldFail(string nationality)
    {
        var context = CreateAuthorizationContext(nationality);

        await _cut.HandleAsync(context);


        Assert.That(context.HasFailed, Is.True);
    }

    [Test]
    public async Task HandleRequirementAsync_IfNationalityIsNull_ShouldFail()
    {
        var context = CreateAuthorizationContext(null!);

        await _cut.HandleAsync(context);

        Assert.That(context.HasFailed, Is.True);
    }

    private AuthorizationHandlerContext CreateAuthorizationContext(string nationality)
    {
        var requirement = new NationalityRequirement(NationalityConstants.AllowedNationalities);

        var claims = new List<Claim>();

        if (nationality is not null)
            claims.Add(new Claim(AppClaimTypesConstants.Nationality, nationality));
                
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "testAuthenticationType"));

        return new AuthorizationHandlerContext([requirement], user, null);
    }

}