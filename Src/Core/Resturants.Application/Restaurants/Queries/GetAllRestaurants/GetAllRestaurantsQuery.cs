using MediatR;
using Resturants.Application.Common;
using Resturants.Application.Restaurants.Dtos;
using Resturants.Domain.Enums;

namespace Resturants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery : IRequest<PageResultsDto<RestaurantDto>>
{
    public bool Includes { get; init; } 
    public string? SearchCritrea { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public string? SortBy {  get; init; }
    public SortingDirection SortingDirection { get; init; } = SortingDirection.Ascending;
}
