using MediatR;
using Resturants.Application.Restaurants.Dtos;
using Resturants.Domain.Entities;
using System.Linq.Expressions;

namespace Resturants.Application.Restaurants.Queries.FindRestaurant;

public class FindRestaurantQuery : IRequest<RestaurantDto>
{
    public int Id { get; set; }
    public bool Includes { get; init; } 
    public Expression<Func<Restaurant, bool>> Criteria { get; set; }
}
