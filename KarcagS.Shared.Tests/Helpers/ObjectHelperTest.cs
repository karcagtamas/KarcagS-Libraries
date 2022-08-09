using KarcagS.Shared.Helpers;

namespace KarcagS.Shared.Tests.Helpers;

public class ObjectHelperTest
{
    [Fact]
    public void IsNull_NullValue_ReturnTrue()
    {
        string? value = null;

        var result = ObjectHelper.IsNull(value);

        Assert.True(result);
    }

    [Fact]
    public void IsNull_NotNullValue_ReturnFalse()
    {
        string? value = "ALMA";

        var result = ObjectHelper.IsNull(value);

        Assert.False(result);
    }

    [Fact]
    public void IsNotNull_NotNullValue_ReturnTrue()
    {
        string? value = "ALMA";

        var result = ObjectHelper.IsNotNull(value);

        Assert.True(result);
    }

    [Fact]
    public void IsNotNulll_NullValue_ReturnFalse()
    {
        string? value = null;

        var result = ObjectHelper.IsNotNull(value);

        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_EmptyList_ReturnTrue()
    {
        List<object> value = new();

        var result = ObjectHelper.IsEmpty(value);

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_NotEmptyList_ReturnFalse()
    {
        List<object> value = new() { "" };

        var result = ObjectHelper.IsEmpty(value);

        Assert.False(result);
    }

    [Fact]
    public void IsNotEmpty_EmptyList_ReturnFalse()
    {
        List<object> value = new();

        var result = ObjectHelper.IsNotEmpty(value);

        Assert.False(result);
    }

    [Fact]
    public void IsNotEmpty_NotEmptyList_ReturnTrue()
    {
        List<object> value = new() { "" };

        var result = ObjectHelper.IsNotEmpty(value);

        Assert.True(result);
    }

    [Fact]
    public void OrElse_NullValue_ReturnElseValue()
    {
        object? value = null;
        object orElse = "else";

        var result = ObjectHelper.OrElse(value, orElse);

        Assert.Equal(orElse, result);
    }

    [Fact]
    public void OrElse_NotNullValue_ReturnOriginalValue()
    {
        object? value = "original";
        object orElse = "else";

        var result = ObjectHelper.OrElse(value, orElse);

        Assert.Equal(value, result);
    }

    [Fact]
    public void OrElseThrow_NullValue_ThrowException()
    {
        object? value = null;

        Assert.Throws<Exception>(() => ObjectHelper.OrElseThrow(value, new Exception()));
    }

    [Fact]
    public void OrElseThrow_NotNullValue_ReturnOriginalValue()
    {
        object? value = "ALMA";

        var result = ObjectHelper.OrElseThrow(value, new Exception());

        Assert.Equal(value, result);
    }
}
