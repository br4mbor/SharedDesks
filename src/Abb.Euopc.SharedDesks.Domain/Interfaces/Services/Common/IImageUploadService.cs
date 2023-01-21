namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Common;

public interface IImageUploadService
{
    Task<string> UploadImageAsync(byte[] image, string associatedEntityName);
    Task<bool> DeleteImageAsync(string imageUrl, string associatedEntityName);
}