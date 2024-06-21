using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resturants.Application.Users.Commands.AssignAndUnAssignRoles;
using Resturants.Application.Users.Commands.UpdateUserDetails;
using Resturants.Domain.Constants;

namespace Resturants.Api.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _mediator;
        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("updateUserDetails")]
        [Authorize]
        public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand updateUserDetailsCommand)
        {
            await _mediator.Send(updateUserDetailsCommand);
            return NoContent();
        }

        [HttpPost("assignRole")]
        [Authorize(Roles = RolesConstants.Admin)]
        public async Task<IActionResult> AssignRole(UserRoleCommand assignRoleCommand)
        {
            assignRoleCommand.SetAssignOrUnAssign(true);
            await _mediator.Send(assignRoleCommand);
            return NoContent();
        }


        [HttpDelete("unassignRole")]
        [Authorize(Roles = RolesConstants.Admin)]
        public async Task<IActionResult> UnAssignRole(UserRoleCommand unAssignRoleCommand)
        {
            unAssignRoleCommand.SetAssignOrUnAssign(false);
            await _mediator.Send(unAssignRoleCommand);
            return NoContent();
        }
    }
}
