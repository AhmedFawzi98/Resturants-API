using MediatR;
using Microsoft.AspNetCore.Http;
using Resturants.Application.Common;

namespace Resturants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommand : IRequest<FileSasUrlDto>
{
    public int RestaurantId { get; init; }
    public IFormFile File { get; init; }
}

