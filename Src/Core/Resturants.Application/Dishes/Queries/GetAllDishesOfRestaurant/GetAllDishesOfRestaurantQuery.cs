using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resturants.Application.Common;
using Resturants.Application.Dtos;
using Resturants.Domain.Enums;

namespace Resturants.Application.Dishes.Queries.GetAllDishesOfRestaurant;

public class GetAllDishesOfRestaurantQuery:IRequest<PageResultsDto<DishDto>>
{
    public int RestaurantId { get; set; }
    public string? SearchCritrea { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public string? SortBy { get; init; }
    public SortingDirection SortingDirection { get; init; } = SortingDirection.Ascending;
}
