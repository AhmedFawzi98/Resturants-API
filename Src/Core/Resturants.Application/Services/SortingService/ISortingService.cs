using Resturants.Domain.Entities;
using System.Linq.Expressions;

namespace Resturants.Application.Services.SortingService;

public interface ISortingService
{
    Expression<Func<Restaurant, object>> GetRestaurantsSortingExpression(string sortBy);
    Expression<Func<Dish, object>> GetDishesSortingExpression(string sortBy);

}
