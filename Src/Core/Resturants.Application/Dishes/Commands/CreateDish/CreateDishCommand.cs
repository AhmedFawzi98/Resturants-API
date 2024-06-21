using MediatR;
using Resturants.Application.Dtos;

namespace Resturants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommand : IRequest<DishDto>
{
    public int RestaurantId { get; private set; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int? KiloCalorie { get; init; }
    public void SetRestaurantId(int id)
    {
        RestaurantId = id;
    }
}
