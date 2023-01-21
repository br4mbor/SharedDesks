using ImageMagick;
using Microsoft.AspNetCore.Components.Forms;

namespace Abb.Euopc.SharedDesks.WebClient.Helpers
{
    internal static class ImageHelper
    {
        public static async Task<byte[]?> ConvertImageAsync(IBrowserFile image)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    await image.OpenReadStream(1024000).CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    using var resizedImage = new MagickImage(stream);

                    //height will be calculated with aspect ratio
                    //resizedImage.Resize(400, 0);
                    resizedImage.Format = MagickFormat.WebP;

                    return resizedImage.ToByteArray();
                }
            }
            catch
            {
                // TODO: log error
                return null;
            }
        }
    }
}
