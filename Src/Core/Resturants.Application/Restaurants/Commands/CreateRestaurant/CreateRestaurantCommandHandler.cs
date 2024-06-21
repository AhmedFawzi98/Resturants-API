using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resturants.Application.Restaurants.Dtos;
using Resturants.Application.Users;
using Resturants.Domain.Entities;
using Resturants.Domain.Interfaces.Repositories;

namespace Resturants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, RestaurantDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    private readonly ILogger<CreateRestaurantCommandHandler> _logger;
    public CreateRestaurantCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, 
        ILogger<CreateRestaurantCommandHandler> logger, IUserContext userContext)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userContext = userContext;
    }

    public async Task<RestaurantDto> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _userContext.GetCurrentUser();

        var newRestuarant = _mapper.Map<Restaurant>(request);
        newRestuarant.OwnerId = currentUser.Id;
        
        await _unitOfWork.Resturants.AddAsync(newRestuarant);
        await _unitOfWork.CommitAsync();

        var addedRestuarant = _mapper.Map<RestaurantDto>(newRestuarant);
        
        _logger.LogInformation("Create Resaurant Command Was Invoked by {username} with id = {userid}, Created restaurant: {@newRestaurant}",currentUser.Name,currentUser.Id, newRestuarant);

        return addedRestuarant;
    }
}
