using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
public class BlobService
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public BlobService(IConfiguration configuration)
    {
        _connectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        _containerName = configuration.GetValue<string>("AzureBlobStorage:ContainerName");
    }


    public async Task<string> UploadFileAsync(IFormFile file)
    {

        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new Exception("Blob connection string is missing. Check User Secrets.");
        }
        var container = new BlobContainerClient(_connectionString, _containerName);
        await container.CreateIfNotExistsAsync();

        var blobName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var blob = container.GetBlobClient(blobName);

        using var stream = file.OpenReadStream();
        await blob.UploadAsync(stream, overwrite: true);

        return blob.Uri.ToString();
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return;

        var container = new BlobContainerClient(_connectionString, _containerName);

        // Extract file name from URL
        var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);

        var blob = container.GetBlobClient(fileName);

        await blob.DeleteIfExistsAsync();
    }
}