using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;

namespace Blog.Data
{
    public class BlobStorageSettings
    {
        public string? BlobContainerName { get; set; }
        public string? StorageConnectionString { get; set; }
    }

    public class BlobStorageService
    {
        private readonly BlobStorageSettings _blobStorageSettings;

        public BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettings)
        {
            _blobStorageSettings = blobStorageSettings.Value;
        }
        public async Task<string> UploadFileToBlobAsync(IBrowserFile file)
        {
            var blobServiceClient = new BlobServiceClient(_blobStorageSettings.StorageConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.BlobContainerName);

            await using var memoryStream = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(memoryStream);

            var blobClient = blobContainerClient.GetBlobClient(Guid.NewGuid().ToString());
            memoryStream.Position = 0;

            await blobClient.UploadAsync(memoryStream, true);

            return blobClient.Uri.ToString();
        }
    }
}
