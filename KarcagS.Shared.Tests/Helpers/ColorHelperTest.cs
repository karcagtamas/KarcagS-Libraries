using KarcagS.Shared.Helpers;

namespace KarcagS.Shared.Tests.Helpers;

public class ColorHelperTest
{
    [Fact]
    public void GetForegroundColor_Black_ResultWhite()
    {
        var result = ColorHelper.GetForegroundColor("#000000");

        Assert.Equal("#fff", result);
    }

    [Fact]
    public void GetForegroundColor_White_ResultBlack()
    {
        var result = ColorHelper.GetForegroundColor("#ffffff");

        Assert.Equal("#000", result);
    }
}
