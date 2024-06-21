using FluentValidation;

namespace Resturants.Application.Users.Commands.UpdateUserDetails;

public class UpdateUserDetailsCommandValidator : AbstractValidator<UpdateUserDetailsCommand>
{
    public UpdateUserDetailsCommandValidator()
    {
        RuleFor(u => u.DateOfBirth).LessThan(new DateOnly(2006, 1, 1))
            .WithMessage("user date of birth cant be after 2006 (user must be above 18)");
    }
}
