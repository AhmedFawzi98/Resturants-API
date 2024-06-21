using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Resturants.Domain.Entities;
using Resturants.Domain.Exceptions;

namespace Resturants.Application.Users.Commands.UpdateUserDetails;

public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand>
{
    private readonly ILogger<UpdateUserDetailsCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger, IUserContext userContext, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userContext = userContext;
        _userManager = userManager;
    }

    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _userContext.GetCurrentUser();

        var dbUser = await _userManager.FindByIdAsync(currentUser.Id);
        if (dbUser is null)
            throw new NotFoundException(nameof(ApplicationUser), currentUser.Id);

        dbUser.DateOfBirth = request.DateOfBirth;
        dbUser.Nationality = request.Nationality;

        await _userManager.UpdateAsync(dbUser);
        _logger.LogInformation("user with id = {userId} is being update, updated info: {@updateedInfo}", currentUser.Id, request);
    }
}
