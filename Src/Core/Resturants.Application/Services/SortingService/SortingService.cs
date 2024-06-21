using Resturants.Domain.Entities;
using System.Linq.Expressions;

namespace Resturants.Application.Services.SortingService;

public class SortingService : ISortingService
{
    Dictionary<string, Expression<Func<Restaurant, object>>> restaurantExpressionsDictionary = new()
    {
        { nameof(Restaurant.Name).ToLower(), r => r.Name },
        { nameof(Restaurant.Address.City).ToLower(), r => r.Address.City },
    };

    Dictionary<string, Expression<Func<Dish, object>>> dishesExpressionsDictionary = new()
    {
        { nameof(Dish.Name).ToLower(), r => r.Name },
        { nameof(Dish.Price).ToLower(), r => r.Price },
        { nameof(Dish.KiloCalorie).ToLower(), r => r.KiloCalorie }
    };

    public Expression<Func<Restaurant, object>> GetRestaurantsSortingExpression(string sortBy)
    {
        return restaurantExpressionsDictionary[sortBy.ToLower()];
    }

    public Expression<Func<Dish, object>> GetDishesSortingExpression(string sortBy)
    {
        return dishesExpressionsDictionary[sortBy.ToLower()];
    }
}
