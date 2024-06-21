
using MediatR;

namespace Resturants.Application.Users.Commands.AssignAndUnAssignRoles;

public class UserRoleCommand : IRequest
{
    public string UserEmail { get; init; }
    public string RoleName { get; init; }
    public bool IsAssingingRole { get; private set; }
    public void SetAssignOrUnAssign(bool isAssigningRole)
    {
        IsAssingingRole = isAssigningRole;
    }
}
