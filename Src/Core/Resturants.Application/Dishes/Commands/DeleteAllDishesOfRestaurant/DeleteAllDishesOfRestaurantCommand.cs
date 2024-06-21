using MediatR;

namespace Resturants.Application.Dishes.Commands.DeleteAllDishes;

public class DeleteAllDishesOfRestaurantCommand:IRequest
{
    public int RestaurantId { get; init; }
    public string[] Includes { get; init; } = null;

}
