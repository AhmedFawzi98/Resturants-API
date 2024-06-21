using MediatR;
using Resturants.Application.Restaurants.Dtos;

namespace Resturants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommand:IRequest<RestaurantDto>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public bool HasDelivery { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactNumber { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
}
