using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Resturants.Application.Common;
using Resturants.Application.Restaurants.Dtos;
using Resturants.Domain.Entities;
using Resturants.Domain.Interfaces.Repositories;

namespace Resturants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private readonly int[] allowedPageSizes = [5, 10, 15, 20, 50];

    private readonly string[] allowedSortingByProperties =
        [
            nameof(RestaurantDto.Name),
            nameof(RestaurantDto.City),
        ];


    public GetAllRestaurantsQueryValidator()
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
            if (!allowedSortingByProperties.Contains(value))
                context.AddFailure($"sorting is optional, but if sorting is requestd, it must be by one of these properties [{string.Join(",", allowedSortingByProperties)}]");
        })
        .When(r => r.SortBy != null);

    }
}
