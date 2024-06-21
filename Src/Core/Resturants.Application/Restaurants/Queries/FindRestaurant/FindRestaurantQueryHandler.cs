using AutoMapper;
using MediatR;
using Resturants.Application.Restaurants.Dtos;
using Resturants.Domain.Entities;
using Resturants.Domain.Exceptions;
using Resturants.Domain.Interfaces.Repositories;
using Resturants.Domain.Interfaces.Services;

namespace Resturants.Application.Restaurants.Queries.FindRestaurant;

public class FindRestaurantQueryHandler : IRequestHandler<FindRestaurantQuery, RestaurantDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public FindRestaurantQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task<RestaurantDto> Handle(FindRestaurantQuery request, CancellationToken cancellationToken)
    {
        string[] includes = null!;
        if (request.Includes)
            includes = ["Dishes"];

        var restaurant = await _unitOfWork.Resturants.FindAsync(request.Criteria, includes)
            ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
        restaurantDto.SasLogoUrl = _blobStorageService.GetBlobSasUrl(restaurantDto.LogoUrl);

        return restaurantDto;
    }
}
