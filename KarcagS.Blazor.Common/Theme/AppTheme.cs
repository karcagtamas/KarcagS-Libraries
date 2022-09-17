using MudBlazor;
using MudBlazor.Utilities;

namespace KarcagS.Blazor.Common.Theme;

public class AppTheme
{
    public MudTheme Theme { get; }
    public AppColorPalette ColorPalette { get; }
    public AppColorPalette DarkColorPalette { get; }

    public AppTheme(AppColorPalette palette, AppColorPalette darkPalette, int drawerWidthLeft = 260, int drawerWidthRight = 300)
    {
        ColorPalette = palette;
        DarkColorPalette = darkPalette;
        Theme = new()
        {
            Palette = new Palette
            {
                Primary = palette.MainColor,
                Secondary = palette.SecondaryColor,
                Tertiary = palette.TertiaryColor,
                Info = palette.InfoColor,
                Success = palette.SuccessColor,
                Warning = palette.WarningColor,
                Error = palette.ErrorColor,
                Divider = palette.MainColor,
                DrawerBackground = palette.MainColor.ColorLighten(.71),
                DrawerText = palette.MainColor,
                DrawerIcon = palette.MainColor,
                ActionDefault = palette.MainColor,
                AppbarText = palette.White
            },
            PaletteDark = new Palette
            {
                Primary = darkPalette.MainColor,
                Secondary = darkPalette.SecondaryColor,
                Tertiary = darkPalette.TertiaryColor,
                Info = darkPalette.InfoColor,
                Success = darkPalette.SuccessColor,
                Warning = darkPalette.WarningColor,
                Error = darkPalette.ErrorColor,
                Divider = darkPalette.MainColor,
                DrawerBackground = darkPalette.MainColor.ColorLighten(.71),
                DrawerText = darkPalette.MainColor,
                DrawerIcon = darkPalette.MainColor,
                ActionDefault = darkPalette.MainColor,
                AppbarText = darkPalette.White
            },
            LayoutProperties = new LayoutProperties
            {
                DrawerWidthLeft = $"{drawerWidthLeft}px",
                DrawerWidthRight = $"{drawerWidthRight}px"
            },
            Typography = new Typography
            {
                Default = new Default { FontFamily = new[] { "Bree Serif", "serif" } },
                Button = new Button
                {
                    TextTransform = "none"
                }
            }
        };
    }

    public AppTheme(MudTheme theme)
    {
        Theme = theme;
        ColorPalette = new AppColorPalette(theme.Palette);
        DarkColorPalette = new AppColorPalette(theme.PaletteDark);
    }

    public class AppColorPalette
    {
        public string MainColorValue { get => MainColor.Value; }
        public MudColor MainColor { get; set; } = new("#0D2971");
        public MudColor SecondaryColor { get; set; } = new("#581043");
        public MudColor TertiaryColor { get; set; } = new("#361B3A");
        public MudColor WarningColor { get; set; } = new("#C07C38");
        public MudColor ErrorColor { get; set; } = new("#850525");
        public MudColor InfoColor { get; set; } = new("#66ADA1");
        public MudColor SuccessColor { get; set; } = new("#40921A");
        public MudColor White { get; set; } = new("#FFFFFF");

        public AppColorPalette()
        {

        }

        public AppColorPalette(Palette palette)
        {
            MainColor = palette.Primary;
            SecondaryColor = palette.Secondary;
            TertiaryColor = palette.Tertiary;
            WarningColor = palette.Warning;
            ErrorColor = palette.Error;
            InfoColor = palette.Info;
            SuccessColor = palette.Success;
            White = palette.White;
        }
    }
}
