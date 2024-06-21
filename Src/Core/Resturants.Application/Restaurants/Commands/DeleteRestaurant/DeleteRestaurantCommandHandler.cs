using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resturants.Application.Restaurants.Commands.CreateRestaurant;
using Resturants.Domain.Entities;
using Resturants.Domain.Enums;
using Resturants.Domain.Exceptions;
using Resturants.Domain.Interfaces.Repositories;
using Resturants.Domain.Interfaces.Services;

namespace Resturants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteRestaurantCommandHandler> _logger;
    private readonly IRestaurantAuthoriazationService _restaurantAuthoriazationService;
    public DeleteRestaurantCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteRestaurantCommandHandler> logger
        ,IRestaurantAuthoriazationService restaurantAuthoriazationService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _restaurantAuthoriazationService = restaurantAuthoriazationService;

    }

    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {

        var restaurantToBeDeleted = await _unitOfWork.Resturants.FindAsync(r => r.Id == request.Id) 
            ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        if (!_restaurantAuthoriazationService.Authorize(restaurantToBeDeleted, CrudOperations.delete))
            throw new ForbiddenException();

        _unitOfWork.Resturants.Delete(restaurantToBeDeleted);
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Delete Resaurant Command Was Invoked, deleted restaurant: {@deletedRestaurant}", restaurantToBeDeleted);

    }
}
