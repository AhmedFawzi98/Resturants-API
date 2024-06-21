using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Resturants.Application.Common;
using Resturants.Application.Dishes.Queries.GetAllDishesOfRestaurant;
using Resturants.Application.Dtos;
using Resturants.Application.Restaurants.Dtos;
using Resturants.Domain.Entities;
using Resturants.Domain.Interfaces.Repositories;

namespace Resturants.Application.Dishes.Queries.GetAllDishesOfRestaurant;

public class GetAllDishesOfRestaurantQueryValidator : AbstractValidator<GetAllDishesOfRestaurantQuery>
{
    private readonly int[] allowedPageSizes = [5, 10, 15, 20, 50];
    private readonly string[] allowedSortingByProperties =
    [
        nameof(DishDto.Name).ToLower(),
        nameof(DishDto.Price).ToLower(),
        nameof(DishDto.KiloCalorie).ToLower(),
    ];

    public GetAllDishesOfRestaurantQueryValidator()
    {
        RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be equal or greater than 1");
        

        RuleFor(r => r.PageSize).Custom((value, context) =>
        {
            if (!allowedPageSizes.Contains(value))
                context.AddFailure("PageSize", $"page size must be either [{string.Join(",", allowedPageSizes)}]");
        });

        RuleFor(r => r.SortBy).Custom((value, context) =>
        {
            if (!allowedSortingByProperties.Contains(value.ToLower()))
                context.AddFailure($"sorting is optional, but if sorting is requestd, it must be by one of these properties [{string.Join(",", allowedSortingByProperties)}]");
        })
        .When(r => r.SortBy != null);
    }
}
