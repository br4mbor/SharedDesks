using Abb.Euopc.SharedDesks.WebClient.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Shared;

public partial class MainLayout
{
    private readonly MudTheme _abbTheme = new AbbTheme();
    private bool _drawerOpen = false;
    private User? User { get; set; }
    private string Photo { get; set; } = string.Empty;
    public string? CurrentUserEmail => User?.Mail;

    [Inject]
    private AzureAdService AzureAdService { get; set; } = default!;

    [Inject]
    private MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            User = await AzureAdService.GetLoggedUser();
            Photo = await AzureAdService.GetPhoto();
        }
        catch (Exception ex)
        {
            ConsentHandler.HandleException(ex);
        }
    }

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}
