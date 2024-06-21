using Resturants.Domain.Entities;
using Resturants.Domain.Enums;

namespace Resturants.Domain.Interfaces.Services;

public interface IRestaurantAuthoriazationService
{
    bool Authorize(Restaurant restaurant, CrudOperations operation);
}
