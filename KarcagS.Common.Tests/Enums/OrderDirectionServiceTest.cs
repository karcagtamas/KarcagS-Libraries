using System;
using KarcagS.Common.Enums;

namespace KarcagS.Common.Tests.Enums;

public class OrderDirectionServiceTest
{
    [Theory]
    [InlineData(OrderDirection.Ascend, "asc")]
    [InlineData(OrderDirection.Descend, "desc")]
    [InlineData(OrderDirection.None, "none")]
    public void GetValue_GiveValidOrder_ReturnValidDirectionString(OrderDirection direction, string expectedResult)
    {
        var result = OrderDirectionService.GetValue(direction);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetValue_GiveInvalidOrder_ThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => OrderDirectionService.GetValue((OrderDirection)4));
    }

    [Theory]
    [InlineData("asc", OrderDirection.Ascend)]
    [InlineData("desc", OrderDirection.Descend)]
    [InlineData("none", OrderDirection.None)]
    public void ValueToKey_GiveValidValue_ReturnValidDirection(string directionValue, OrderDirection expectedResult)
    {
        var result = OrderDirectionService.ValueToKey(directionValue);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ValueToKey_GiveInvalidValue_ThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => OrderDirectionService.ValueToKey("alma"));
    }
}