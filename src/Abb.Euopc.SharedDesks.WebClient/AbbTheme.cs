using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient;

internal sealed class AbbTheme : MudTheme
{
    public AbbTheme()
    {
        Palette = new Palette()
        {
            ActionDefault = "#BABABA",
            ActionDisabled = "#1F1F1F20",
            ActionDisabledBackground = "#EBEBEB",
            AppbarBackground = "#FFFFFF",
            AppbarText = "#1F1F1F",
            Background = "#EBEBEB",
            Black = "#000000",
            White = "#FFFFFF",
            Primary = "#3366FF",
            PrimaryDarken = "#2A35FF",
            PrimaryLighten = "#4C85FF",
            PrimaryContrastText = "#FFFFFF",
            SecondaryContrastText = "#696969",
            TextPrimary = "#1F1F1F",
            TextSecondary = "#696969",

            Warning = "#FF7300",
            WarningContrastText = "#FFFFFF",
            Error = "#F03040",
            ErrorContrastText = "#FFFFFF",
            Success = "#0CA919",
            SuccessContrastText = "#FFFFFF",
            Info = "#3366FF",
            InfoContrastText = "#FFFFFF"
        };

        LayoutProperties = new LayoutProperties
        {

        };

        Typography = new Typography
        {
            Default = new Default
            {
                FontFamily = new[] { "ABBvoice", "sans-serif" },
                FontSize = "14px",
                LineHeight = 1.5
            }
        };
    }
}
