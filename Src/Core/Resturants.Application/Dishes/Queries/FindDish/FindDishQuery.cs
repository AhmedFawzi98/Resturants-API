using MediatR;
using Resturants.Application.Dtos;
using Resturants.Domain.Entities;
using System.Linq.Expressions;

namespace Resturants.Application.Dishes.Queries.GetDishById;

public class FindDishQuery:IRequest<DishDto>
{
    public int DishId { get; set; }
    public int RestaurantId { get; set; }
    public string[] Includes { get; init; } = null;
    public Expression<Func<Dish, bool>> Criteria { get; set; }
}
