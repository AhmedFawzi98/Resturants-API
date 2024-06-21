namespace Resturants.Domain.Interfaces.Services;

public interface IBlobStorageService
{
    Task<String> UploadToBlobAsync(string fileName, Stream file, string contentType);
    Task DeleteBlobAsync(string logoUrl);
    string GetBlobSasUrl(string? blobUrl);
}
