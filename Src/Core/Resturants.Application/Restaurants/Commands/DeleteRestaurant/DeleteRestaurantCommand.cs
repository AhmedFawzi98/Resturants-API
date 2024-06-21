using MediatR;

namespace Resturants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommand:IRequest
{ 
    public int Id { get; init; }
}
