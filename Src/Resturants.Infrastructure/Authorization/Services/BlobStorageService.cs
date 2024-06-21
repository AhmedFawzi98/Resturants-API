using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Resturants.Domain.Interfaces.Services;
using Resturants.Infrastructure.Configurations;

namespace Resturants.Infrastructure.Authorization.Services;

internal class BlobStorageService : IBlobStorageService
{
    private readonly BlobStorageSettings _blobStorageSettings;

    public BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettings)
    {
        _blobStorageSettings = blobStorageSettings.Value;
    }

    public async Task DeleteBlobAsync(string logoUrl)
    {
        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);

        string fileName = new Uri(logoUrl).Segments.Last();

        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<string> UploadToBlobAsync(string fileName, Stream file, string contentType)
    {
        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType,
                ContentDisposition = $"inline; filename={fileName}"
            }
        };

        await blobClient.UploadAsync(file);
        var blobUrl = blobClient.Uri.ToString();
        
        return blobUrl;
    }
    public string GetBlobSasUrl(string? blobUrl)
    {
        if (string.IsNullOrEmpty(blobUrl))
            return null;

        var sasBuilder = new BlobSasBuilder()
        {
            BlobName = new Uri(blobUrl).Segments.Last(),
            BlobContainerName = _blobStorageSettings.LogosContainerName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(2)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_blobStorageSettings.AccountName, _blobStorageSettings.AccountKey))
                                 .ToString();

        return $"{blobUrl}?{sasToken}";
    }
}
