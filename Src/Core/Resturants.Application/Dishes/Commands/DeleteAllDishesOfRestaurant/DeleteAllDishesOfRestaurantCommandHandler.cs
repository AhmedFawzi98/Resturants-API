using MediatR;
using Microsoft.Extensions.Logging;
using Resturants.Application.Dishes.Commands.CreateDish;
using Resturants.Application.Dishes.Commands.DeleteAllDishes;
using Resturants.Domain.Entities;
using Resturants.Domain.Enums;
using Resturants.Domain.Exceptions;
using Resturants.Domain.Interfaces.Repositories;
using Resturants.Domain.Interfaces.Services;

namespace Resturants.Application.Dishes.Commands.DeleteAllDishesOfRestaurant;

public class DeleteAllDishesOfRestaurantCommandHandler : IRequestHandler<DeleteAllDishesOfRestaurantCommand>
{
    private readonly ILogger<CreaterDishCommandHanlder> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantAuthoriazationService _restaurantAuthoriazationService;

    public DeleteAllDishesOfRestaurantCommandHandler(ILogger<CreaterDishCommandHanlder> logger, IUnitOfWork unitOfWork
        , IRestaurantAuthoriazationService restaurantAuthoriazationService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _restaurantAuthoriazationService =  restaurantAuthoriazationService;
    }

    public async Task Handle(DeleteAllDishesOfRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.Resturants.FindAsync(r => r.Id == request.RestaurantId, request.Includes)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!_restaurantAuthoriazationService.Authorize(restaurant, CrudOperations.update))
            throw new ForbiddenException();


        _unitOfWork.Dishes.DeleteRange(restaurant.Dishes);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Delete All Dishes Of Restaurant Command Was Invoked, for restaurant: {@Restaurant}", request.RestaurantId);

    }
}
