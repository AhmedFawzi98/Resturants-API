using Microsoft.AspNetCore.Http;
using Moq;
using Resturants.Application.Users;
using Resturants.Domain.Constants;
using System.Security.Claims;

namespace Resturants.Application.Tests.Users;

[TestFixture]
public class UserContextTests
{
    private Mock<IHttpContextAccessor> httpContextAccessorMock;
    private IUserContext _cut;

    [SetUp]
    public void SetUp()
    {
        httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _cut = new UserContext(httpContextAccessorMock.Object);
    }


    [TestCase("1", "user1", new string[] { RolesConstants.Admin })]
    [TestCase("12", "user2", new string[] { RolesConstants.Owner })]
    public void GetCurrentUser_IfUserIsAuthenticated_ShallReturnCurrentUserCorrectly(string id, string name, string[] roles)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier,id),
            new(ClaimTypes.Name,name),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "testAuthenticationType"));

        httpContextAccessorMock.Setup(c => c.HttpContext)
            .Returns(new DefaultHttpContext() { User = user });

        var currentUser = _cut.GetCurrentUser();

        Assert.That(currentUser, Is.Not.Null);
        Assert.That(currentUser.Id, Is.EqualTo(id));
        Assert.That(currentUser.Name, Is.EqualTo(name));
        Assert.That(currentUser.Roles, Is.EqualTo(roles));
    }

    [Test]
    public void GetCurrentUser_IfUserIsNull_ShallThrowInvalidOperationException()
    {
        httpContextAccessorMock.Setup(c => c.HttpContext)
                    .Returns((HttpContext) null);

        Assert.Throws<InvalidOperationException>(() => _cut.GetCurrentUser());
    }


}