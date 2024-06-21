using AutoMapper;
using MediatR;
using Resturants.Application.Dtos;
using Resturants.Domain.Entities;
using Resturants.Domain.Exceptions;
using Resturants.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace Resturants.Application.Dishes.Queries.GetDishById;

public class FindDishQueryHandler:IRequestHandler<FindDishQuery,DishDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FindDishQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DishDto> Handle(FindDishQuery request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.Resturants.FindAsync(r => r.Id == request.RestaurantId)
           ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = await _unitOfWork.Dishes.FindAsync(request.Criteria, request.Includes)
           ?? throw new NotFoundException(nameof(Dish), request.DishId.ToString());

        var dishDto = _mapper.Map<DishDto>(dish);

        return dishDto;
    }
}
