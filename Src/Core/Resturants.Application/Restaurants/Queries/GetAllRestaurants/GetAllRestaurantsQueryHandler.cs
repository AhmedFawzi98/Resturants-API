using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Resturants.Application.Common;
using Resturants.Application.Restaurants.Dtos;
using Resturants.Application.Services.SortingService;
using Resturants.Domain.Entities;
using Resturants.Domain.Interfaces.Repositories;
using Resturants.Domain.Interfaces.Services;
using System.Linq.Expressions;

namespace Resturants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, PageResultsDto<RestaurantDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISortingService _sortingService;
    private readonly IBlobStorageService _blobStorageService;

    public GetAllRestaurantsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork,
        ISortingService sortingService, IBlobStorageService blobStorageService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _sortingService = sortingService;
        _blobStorageService = blobStorageService;
    }

    public async Task<PageResultsDto<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Restaurant> restaurants;
        int totalCount;

        string[] includes = null!;
        if(request.Includes)
        {
            includes = ["Dishes"];
        }

        Expression<Func<Restaurant,object>> sortingExpression = null;
        if (request.SortBy != null)
            sortingExpression = _sortingService.GetRestaurantsSortingExpression(request.SortBy);

        if (!string.IsNullOrEmpty(request.SearchCritrea))
        {
            var lowerSearchCritrea = request.SearchCritrea.Trim().ToLower();

            (restaurants, totalCount) = await _unitOfWork.Resturants.GetAllAsync(
                request.PageSize, request.PageNumber,
                request.SortingDirection, sortingExpression,
                r => r.Name.ToLower().Contains(lowerSearchCritrea) 
                || r.Category.ToLower().Contains(lowerSearchCritrea)
                || (r.Address != null && r.Address.City.ToLower().Contains(lowerSearchCritrea))
                || (r.Address != null && r.Address.Street.ToLower().Contains(lowerSearchCritrea))
                , includes);
        }
        else
            (restaurants, totalCount) = await _unitOfWork.Resturants.GetAllAsync(request.PageSize, request.PageNumber
                , request.SortingDirection, sortingExpression, null, includes);

        var resturantsDtos = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        foreach (var dto in resturantsDtos)
        {
            if (!string.IsNullOrEmpty(dto.LogoUrl))
            {
                dto.SasLogoUrl = _blobStorageService.GetBlobSasUrl(dto.LogoUrl);
            }
        }

        return new PageResultsDto<RestaurantDto>(resturantsDtos, totalCount, request.PageNumber, request.PageSize);
    }
}
