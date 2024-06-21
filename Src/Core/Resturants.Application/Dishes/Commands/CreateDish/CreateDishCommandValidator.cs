using FluentValidation;

namespace Resturants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(d => d.Price).GreaterThanOrEqualTo(0).WithMessage("price must be a non-negative number");

        RuleFor(d => d.KiloCalorie).GreaterThanOrEqualTo(0).WithMessage("KiloCalorie must be a non-negative number");

        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("must provide name")
            .Length(3, 100).WithMessage("name length must be between 3 chars and 100 chars");

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("must provide description");
    }
}
