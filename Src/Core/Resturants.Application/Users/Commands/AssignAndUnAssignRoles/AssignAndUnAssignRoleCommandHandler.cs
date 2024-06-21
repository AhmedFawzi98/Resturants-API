using MediatR;
using Microsoft.AspNetCore.Identity;
using Resturants.Domain.Entities;
using Resturants.Domain.Exceptions;

namespace Resturants.Application.Users.Commands.AssignAndUnAssignRoles;

public class AssignAndUnAssignRoleCommandHandler : IRequestHandler<UserRoleCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AssignAndUnAssignRoleCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Handle(UserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(ApplicationUser), request.UserEmail);

        var role = await _roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

        if (request.IsAssingingRole)
            await _userManager.AddToRoleAsync(user, role.Name!);
        else
            await _userManager.RemoveFromRoleAsync(user, role.Name!);
    }
}
