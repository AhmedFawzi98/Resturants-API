using Resturants.Application.Users;
using Resturants.Domain.Constants;
using Resturants.Domain.Entities;
using Resturants.Domain.Enums;
using Resturants.Domain.Interfaces.Services;

namespace Resturants.Infrastructure.Authorization.Services;

internal class RestaurantAuthoriazationService : IRestaurantAuthoriazationService
{
    private readonly IUserContext _userContext;

    public RestaurantAuthoriazationService(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public bool Authorize(Restaurant restaurant, CrudOperations operation)
    {
        var currentUser = _userContext.GetCurrentUser();
        if(
            (operation == CrudOperations.update)
            && currentUser.IsInRole(RolesConstants.Owner)
            && currentUser.Id == restaurant.OwnerId)
        {
            return true;
        }


        if(
            operation == CrudOperations.delete 
            && (
                  (currentUser.IsInRole(RolesConstants.Owner) && currentUser.Id == restaurant.OwnerId) 
                  || currentUser.IsInRole(RolesConstants.Admin)
               )
          )
        {
            return true;
        }

        return false;

    }
}
