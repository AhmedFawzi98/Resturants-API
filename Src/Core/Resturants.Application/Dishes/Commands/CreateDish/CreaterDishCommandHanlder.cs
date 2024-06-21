using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resturants.Application.Dtos;
using Resturants.Domain.Entities;
using Resturants.Domain.Enums;
using Resturants.Domain.Exceptions;
using Resturants.Domain.Interfaces.Repositories;
using Resturants.Domain.Interfaces.Services;

namespace Resturants.Application.Dishes.Commands.CreateDish;

public class CreaterDishCommandHanlder : IRequestHandler<CreateDishCommand, DishDto>
{
    private readonly ILogger<CreaterDishCommandHanlder> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRestaurantAuthoriazationService _restaurantAuthoriazationService;

    public CreaterDishCommandHanlder(ILogger<CreaterDishCommandHanlder> logger, IUnitOfWork unitOfWork
        , IMapper mapper, IRestaurantAuthoriazationService restaurantAuthoriazationService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _restaurantAuthoriazationService = restaurantAuthoriazationService;
    }

    public async Task<DishDto> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.Resturants.FindAsync(r => r.Id == request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!_restaurantAuthoriazationService.Authorize(restaurant, CrudOperations.update))
            throw new ForbiddenException();

        var dishToBeAdded = _mapper.Map<Dish>(request);
        await _unitOfWork.Dishes.AddAsync(dishToBeAdded);
        await _unitOfWork.CommitAsync();

        var addedDish = _mapper.Map<DishDto>(dishToBeAdded);
        _logger.LogInformation("Create Dish Command Was Invoked, Created dish: {@newdish}", addedDish);

        return addedDish;

    }
}
