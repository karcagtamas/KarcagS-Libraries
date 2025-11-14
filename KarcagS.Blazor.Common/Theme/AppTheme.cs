using MudBlazor;
using MudBlazor.Utilities;

namespace KarcagS.Blazor.Common.Theme;

public class AppTheme
{
    public MudTheme Theme { get; }
    public AppColorPalette ColorPalette { get; }
    public AppColorPalette DarkColorPalette { get; }

    public AppTheme(AppColorPalette palette, AppColorPalette darkPalette, int drawerWidthLeft = 260,
        int drawerWidthRight = 300)
    {
        ColorPalette = palette;
        DarkColorPalette = darkPalette;
        Theme = new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Primary = palette.MainColor,
                Secondary = palette.SecondaryColor,
                Tertiary = palette.TertiaryColor,
                Info = palette.InfoColor,
                Success = palette.SuccessColor,
                Warning = palette.WarningColor,
                Error = palette.ErrorColor,
                Divider = palette.MainColor,
                DrawerBackground = palette.DrawerBackground,
                DrawerText = palette.MainColor,
                DrawerIcon = palette.MainColor,
                ActionDefault = palette.MainColor,
                AppbarText = palette.White
            },
            PaletteDark = ConvertToDarkTheme(new PaletteDark
            {
                Primary = darkPalette.MainColor,
                Secondary = darkPalette.SecondaryColor,
                Tertiary = darkPalette.TertiaryColor,
                Info = darkPalette.InfoColor,
                Success = darkPalette.SuccessColor,
                Warning = darkPalette.WarningColor,
                Error = darkPalette.ErrorColor,
                Divider = darkPalette.MainColor,
                DrawerBackground = darkPalette.DrawerBackground,
                DrawerText = darkPalette.MainColor,
                DrawerIcon = darkPalette.MainColor,
                ActionDefault = darkPalette.MainColor,
                AppbarText = darkPalette.White,
            }),
            LayoutProperties = new LayoutProperties
            {
                DrawerWidthLeft = $"{drawerWidthLeft}px",
                DrawerWidthRight = $"{drawerWidthRight}px"
            },
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = ["Bree Serif", "serif"],
                },
                Button = new ButtonTypography
                {
                    TextTransform = "none",
                },
            },
        };
    }

    public AppTheme(MudTheme theme)
    {
        Theme = theme;
        ColorPalette = new AppColorPalette(theme.PaletteLight);
        DarkColorPalette = new AppColorPalette(theme.PaletteDark);
    }

    public class AppColorPalette
    {
        public string MainColorValue => MainColor.Value;

        public MudColor MainColor { get; set; } = "#0D2971";
        public MudColor SecondaryColor { get; set; } = "#581043";
        public MudColor TertiaryColor { get; set; } = "#361B3A";
        public MudColor WarningColor { get; set; } = "#C07C38";
        public MudColor ErrorColor { get; set; } = "#850525";
        public MudColor InfoColor { get; set; } = "#66ADA1";
        public MudColor SuccessColor { get; set; } = "#40921A";
        public MudColor White { get; set; } = "#FFFFFF";

        public MudColor DrawerBackground { get; set; } = new MudColor("#0D2971").ColorLighten(.71);

        public AppColorPalette()
        {
        }

#pragma warning disable CS0618
        public AppColorPalette(Palette palette)
#pragma warning restore CS0618
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

    public static PaletteDark ConvertToDarkTheme(PaletteDark palette)
    {
        // palette.Primary = "#776be7";
        palette.Black = "#27272f";
        palette.Background = "#32333d";
        palette.BackgroundGray = "#27272f";
        palette.Surface = "#373740";
        // palette.DrawerBackground = "#27272f";
        // palette.DrawerText = "rgba(255,255,255, 0.50)";
        // palette.DrawerIcon = "rgba(255,255,255, 0.50)";
        palette.AppbarBackground = "#27272f";
        // palette.AppbarText = "rgba(255,255,255, 0.70)";
        palette.TextPrimary = "rgba(255,255,255, 0.70)";
        palette.TextSecondary = "rgba(255,255,255, 0.50)";
        // palette.ActionDefault = "#adadb1";
        palette.ActionDisabled = "rgba(255,255,255, 0.26)";
        palette.ActionDisabledBackground = "rgba(255,255,255, 0.12)";
        // palette.Divider = "rgba(255,255,255, 0.12)";
        palette.DividerLight = "rgba(255,255,255, 0.06)";
        palette.TableLines = "rgba(255,255,255, 0.12)";
        palette.TableStriped = "rgba(255,255,255, 0.2)";
        palette.LinesDefault = "rgba(255,255,255, 0.12)";
        palette.LinesInputs = "rgba(255,255,255, 0.3)";
        palette.TextDisabled = "rgba(255,255,255, 0.2)";
        // palette.Info = "#3299ff";
        // palette.Success = "#0bba83";
        // palette.Warning = "#ffa800";
        // palette.Error = "#f64e62";
        palette.Dark = "#27272f";
        return palette;
    }
}