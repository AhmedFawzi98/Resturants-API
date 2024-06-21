namespace Resturants.Infrastructure.Configurations;

public class BlobStorageSettings
{
    public const string BlobStorage = "BlobStorage";
    public string ConnectionString {  get; set; }
    public string LogosContainerName { get; set; }
    public string AccountName { get; set; }
    public string AccountKey { get; set; }
}
