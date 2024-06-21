using MediatR;
using Resturants.Application.Common;
using Resturants.Domain.Entities;
using Resturants.Domain.Enums;
using Resturants.Domain.Exceptions;
using Resturants.Domain.Interfaces.Repositories;
using Resturants.Domain.Interfaces.Services;

namespace Resturants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommandHandler : IRequestHandler<UploadRestaurantLogoCommand, FileSasUrlDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantAuthoriazationService _restaurantAuthoriazationService;
    private readonly IBlobStorageService _blobStorageService;

    public UploadRestaurantLogoCommandHandler(IUnitOfWork unitOfWork, IRestaurantAuthoriazationService authorizationService, IBlobStorageService blobStorageService)
    {
        _unitOfWork = unitOfWork;
        _restaurantAuthoriazationService = authorizationService;
        _blobStorageService = blobStorageService;
    }

    public async Task<FileSasUrlDto> Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        var restuarantToUpdate = await _unitOfWork.Resturants.FindAsync(r => r.Id == request.RestaurantId)
           ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!_restaurantAuthoriazationService.Authorize(restuarantToUpdate, CrudOperations.update))
            throw new ForbiddenException();

        using var stream = request.File.OpenReadStream(); 
        var fileName = $"{Guid.NewGuid().ToString()}_{request.File.FileName}";

        if(!string.IsNullOrEmpty(restuarantToUpdate.LogoUrl))
        {
            await _blobStorageService.DeleteBlobAsync(restuarantToUpdate.LogoUrl);
        }

        string logoUrl = await _blobStorageService.UploadToBlobAsync(fileName, stream, request.File.ContentType);
        restuarantToUpdate.LogoUrl = logoUrl;

        await _unitOfWork.CommitAsync();

        string SasLogoUrl = _blobStorageService.GetBlobSasUrl(restuarantToUpdate.LogoUrl);

        return new FileSasUrlDto() { SasUrl = SasLogoUrl};
    }
}
