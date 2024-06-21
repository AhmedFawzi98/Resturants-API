using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resturants.Application.Restaurants.Commands.CreateRestaurant;
using Resturants.Application.Restaurants.Dtos;
using Resturants.Domain.Entities;
using Resturants.Domain.Enums;
using Resturants.Domain.Exceptions;
using Resturants.Domain.Interfaces.Repositories;
using Resturants.Domain.Interfaces.Services;

namespace Resturants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, RestaurantDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateRestaurantCommandHandler> _logger;
    private readonly IRestaurantAuthoriazationService _restaurantAuthoriazationService;
    private readonly IBlobStorageService _blobStorageService;

    public UpdateRestaurantCommandHandler(IMapper mapper, IUnitOfWork unitOfWork
        , ILogger<UpdateRestaurantCommandHandler> logger, IRestaurantAuthoriazationService restaurantAuthoriazationService
        , IBlobStorageService blobStorageService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _restaurantAuthoriazationService = restaurantAuthoriazationService;
        _blobStorageService = blobStorageService;
    }

    public async Task<RestaurantDto> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restuarantToUpdate = await _unitOfWork.Resturants.FindAsync(r => r.Id == request.Id)
           ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        if (!_restaurantAuthoriazationService.Authorize(restuarantToUpdate, CrudOperations.update))
            throw new ForbiddenException();

        
        _mapper.Map(request, restuarantToUpdate);

        _unitOfWork.Resturants.Update(restuarantToUpdate);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Update Resaurant Command Was Invoked, Updated restaurant: {@updatedRestaurant}", restuarantToUpdate);

        var restaurantDto =  _mapper.Map<RestaurantDto>(restuarantToUpdate);

        return restaurantDto;
    }
}





