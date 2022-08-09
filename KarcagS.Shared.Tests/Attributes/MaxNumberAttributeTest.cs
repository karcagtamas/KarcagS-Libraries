using KarcagS.Shared.Attributes;

namespace KarcagS.Shared.Tests.Attributes;

public class MaxNumberAttributeTest
{
    [Theory]
    [InlineData(10, 12, true)]
    [InlineData(2, 5, true)]
    [InlineData(-32, 12, true)]
    [InlineData(23, 23, true)]
    [InlineData(43, 12, false)]
    [InlineData(2, 1, false)]
    [InlineData(null, 1, true)]
    public void MaxNumber_ValidNumbers_AreValid(long? number, long max, bool expectedResult)
    {
        var attr = new MaxNumberAttribute(max);

        var result = attr.IsValid(number);

        Assert.Equal(expectedResult, result);
    }
}