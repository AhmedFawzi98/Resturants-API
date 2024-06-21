using Resturants.Application.Users;
using Resturants.Domain.Constants;

namespace Resturants.Application.Tests.Users;

[TestFixture]
public class CurrentUsersTest
{
    [TestCase(RolesConstants.Admin)]
    [TestCase(RolesConstants.Owner)]
    public void IsInRole_IfRoleIsMatching_ShallReturnTrue(string role)
    {
        var currentser = new CurrentUser("ahmed", "1", [RolesConstants.Admin, RolesConstants.Owner]);

        bool result = currentser.IsInRole(role);

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsInRole_IfRoleIsNotMatching_ShallReturnFalse()
    {
        var currentser = new CurrentUser("ahmed", "1", [RolesConstants.Admin]);

        bool result = currentser.IsInRole(RolesConstants.User);

        Assert.That(result, Is.False);
    }


}
