using KarcagS.Shared.Attributes;

namespace KarcagS.Shared.Tests.Attributes;

public class MinNumberAttributeTest
{
    [Theory]
    [InlineData(10, 6, true)]
    [InlineData(7, 5, true)]
    [InlineData(12, -3, true)]
    [InlineData(44, 44, true)]
    [InlineData(12, 13, false)]
    [InlineData(2, 9, false)]
    [InlineData(null, 1, true)]
    public void MinNumber_ValidNumbers_AreValid(long? number, long min, bool expectedResult)
    {
        var attr = new MinNumberAttribute(min);

        var result = attr.IsValid(number);

        Assert.Equal(expectedResult, result);
    }
}