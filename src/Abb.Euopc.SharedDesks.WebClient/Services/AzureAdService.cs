using System;
using Microsoft.Graph;

namespace Abb.Euopc.SharedDesks.WebClient.Services;

internal sealed class AzureAdService
{
    private User? _loggedUser;
    private string _photo = string.Empty;
    private readonly GraphServiceClient _graphServiceClient;

    public AzureAdService(GraphServiceClient graphServiceClient) => _graphServiceClient = graphServiceClient;

    public async ValueTask<User> GetLoggedUser()
    {
        if (_loggedUser is null)
            _loggedUser = await _graphServiceClient.Me.Request().GetAsync();

        return _loggedUser;
    }

    public async ValueTask<string> GetPhoto()
    {
        if (!string.IsNullOrEmpty(_photo))
            return _photo;

        try
        {
            using (var photoStream = await _graphServiceClient.Me.Photo.Content.Request().GetAsync())
            {
                byte[] photoByte = ((System.IO.MemoryStream)photoStream).ToArray();
                _photo = Convert.ToBase64String(photoByte);
            }
        }
        catch (Exception)
        {
            _photo = string.Empty;
        }

        return _photo;
    }
}

