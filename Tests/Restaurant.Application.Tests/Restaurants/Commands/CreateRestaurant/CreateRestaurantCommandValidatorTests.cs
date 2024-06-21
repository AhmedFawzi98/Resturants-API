using FluentValidation.TestHelper;
using Resturants.Application.Restaurants.Commands.CreateRestaurant;

namespace Resturants.Application.Tests.Restaurants.Commands.CreateRestaurant;

[TestFixture]
public class CreateRestaurantCommandValidatorTests
{
    [Test]
    public void CreateRestaurantCommandValidator_ifValidCommand_ShallNotBeAnyErrors()
    {
        var command = new CreateRestaurantCommand()
        {
            Name = "name",
            Description = "description",
            Category = "category",
            HasDelivery = true,
        };

        var validator = new CreateRestaurantCommandValidator();

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void CreateRestaurantCommandValidator_ifInValidCommand_ShallHaveValidationErrors()
    {
        var command = new CreateRestaurantCommand()
        {
            Name = "f",
            Category = "category",
            HasDelivery = true,
        };

        var validator = new CreateRestaurantCommandValidator();

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(r => r.Name);
        result.ShouldHaveValidationErrorFor(r => r.Description);
    }
}