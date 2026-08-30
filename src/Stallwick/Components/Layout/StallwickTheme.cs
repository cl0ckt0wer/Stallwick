using MudBlazor;

namespace Stallwick.Components.Layout;

public static class StallwickTheme
{
    public static readonly MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2f6f4e",
            Secondary = "#c9772b",
            AppbarBackground = "#2f6f4e",
            Background = "#f7f6f2",
            DrawerBackground = "#ffffff",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#7bc49a",
            Secondary = "#e5a869",
            AppbarBackground = "#1f2723",
        },
        Typography = new Typography
        {
            Default = new DefaultTypography { FontFamily = ["Roboto", "Helvetica", "Arial", "sans-serif"] },
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "8px",
        },
    };
}
