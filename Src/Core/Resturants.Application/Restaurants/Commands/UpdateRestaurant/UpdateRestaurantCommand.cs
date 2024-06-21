using MediatR;
using Resturants.Application.Restaurants.Dtos;

namespace Resturants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommand:IRequest<RestaurantDto> 
{
    public int Id { get; private set; }
    public string Name { get; init; }
    public string Description { get; init; }
    public bool HasDelivery { get; init; }
    public void SetId(int id)
    {
        Id = id;
    }
}
