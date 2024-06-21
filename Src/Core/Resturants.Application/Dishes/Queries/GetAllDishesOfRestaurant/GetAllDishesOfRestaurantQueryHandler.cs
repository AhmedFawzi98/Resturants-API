using AutoMapper;
using MediatR;
using Resturants.Application.Common;
using Resturants.Application.Dtos;
using Resturants.Application.Services.SortingService;
using Resturants.Domain.Entities;
using Resturants.Domain.Exceptions;
using Resturants.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace Resturants.Application.Dishes.Queries.GetAllDishesOfRestaurant;

public class GetAllDishesOfRestaurantQueryHandler : IRequestHandler<GetAllDishesOfRestaurantQuery, PageResultsDto<DishDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISortingService _sortingService;


    public GetAllDishesOfRestaurantQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ISortingService sortingService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sortingService = sortingService;
    }

    public async Task<PageResultsDto<DishDto>> Handle(GetAllDishesOfRestaurantQuery request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.Resturants.FindAsync(r => r.Id == request.RestaurantId)        
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());


        Expression<Func<Dish, object>> sortingExpression = null;
        if (request.SortBy != null)
            sortingExpression = _sortingService.GetDishesSortingExpression(request.SortBy);


        Expression<Func<Dish,bool>> critrea = d => d.RestaurantId == request.RestaurantId;

        if(!string.IsNullOrEmpty(request.SearchCritrea))
        {
            var lowerSearchCritrea = request.SearchCritrea.Trim().ToLower();

            critrea = d => d.RestaurantId == request.RestaurantId 
            && (d.Name.ToLower().Contains(lowerSearchCritrea) || d.Description.ToLower().Contains(lowerSearchCritrea));
        }
            
        (var dishes, var totalCount) = await _unitOfWork.Dishes.GetAllAsync(request.PageSize, request.PageNumber, request.SortingDirection, sortingExpression, critrea);
        
        var dishesDtos = _mapper.Map<IEnumerable<DishDto>>(dishes);

        return new PageResultsDto<DishDto>(dishesDtos, totalCount,request.PageNumber, request.PageSize);
    }
}
