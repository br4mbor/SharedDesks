using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Common;
using Abb.Euopc.SharedDesks.Infrastructure.Options;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Abb.Euopc.SharedDesks.Infrastructure.Services;

internal sealed class AzureCdnService : IImageUploadService
{
    private readonly AzureCdnServiceOptions _options;
    private readonly ILogger _logger;

    public AzureCdnService(IOptions<AzureCdnServiceOptions> options, ILogger<AzureCdnService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> UploadImageAsync(byte[] image, string associatedEntityName)
    {
        _logger.LogInformation($"Uploading image for {associatedEntityName} to Azure");
        try
        {
            var fileName = CreateName();
            var containerName = associatedEntityName.ToLower();
            var container = new BlobContainerClient(_options.ConnectionString, containerName);

            await container.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var blob = container.GetBlobClient(fileName);

            _logger.LogInformation("Created blob client");

            using (var stream = new MemoryStream(image, false))
            {
                await blob.UploadAsync(stream);
            }

            var url = GetUrl(containerName, fileName);
            _logger.LogInformation($"Image succesfully uploaded to Azure. Image URL: {url}");

            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong when uploading image to Azure CDN.");

            return string.Empty;
        }
    }

    public async Task<bool> DeleteImageAsync(string imageUrl, string associatedEntityName)
    {
        _logger.LogInformation($"Deleting image {imageUrl} from Azure");
        try
        {
            var fileName = Path.GetFileName(imageUrl);
            var containerName = associatedEntityName.ToLower();
            var blob = new BlobClient(_options.ConnectionString, containerName, fileName);

            var result = await blob.DeleteIfExistsAsync();

            _logger.LogInformation($"Image succesfully deleted from Azure. Image URL: {imageUrl}");

            return result.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong when deleting image from Azure CDN.");

            return false;
        }
    }

    private string CreateName()
    {
        _logger.LogInformation("Creating name for image");

        return $"{DateTime.Now.Ticks}.webp".ToLower();
    }

    private string GetUrl(string containerName, string fileName)
    {
        return $"{_options.RootUrl.TrimEnd('/')}/{containerName}/{fileName}".ToLower();
    }
}
