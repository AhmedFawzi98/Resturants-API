using FluentValidation;
using Resturants.Application.Restaurants.Dtos;
using System.Runtime.CompilerServices;

namespace Resturants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("must provide name")
            .Length(3, 100).WithMessage("name length must be between 3 chars and 100 chars");

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("must provide description");
    }
}
