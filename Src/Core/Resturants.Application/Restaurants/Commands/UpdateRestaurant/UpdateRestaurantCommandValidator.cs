using FluentValidation;

namespace Resturants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
           .NotEmpty().WithMessage("must provide name")
           .Length(3, 100).WithMessage("name length must be between 3 chars and 100 chars");

        RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("must provide description");
    }
}
