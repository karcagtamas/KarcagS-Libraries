using KarcagS.Shared.Attributes;

namespace KarcagS.Shared.Tests.Attributes;

public class NullablePhoneAttributeTest
{
    [Fact]
    public void NullablePhoneAddress_NullValue_ReturnTrue()
    {
        var attr = new NullablePhoneAttribute();

        var result = attr.IsValid(null);

        Assert.True(result);
    }

    [Fact]
    public void NullablePhoneAddress_NotStringValue_ReturnFalse()
    {
        var attr = new NullablePhoneAttribute();

        var result = attr.IsValid(1);

        Assert.False(result);
    }

    [Theory]
    [InlineData("12323invalid")]
    [InlineData("notvalid1212")]
    public void NullablePhoneAddress_InvalidEmailValue_ReturnFalse(string value)
    {
        var attr = new NullablePhoneAttribute();

        var result = attr.IsValid(value);

        Assert.False(result);
    }

    [Theory]
    [InlineData("06304949494")]
    [InlineData("+12345678910")]
    public void NullablePhoneAddress_ValidEmailValue_ReturnTrue(string value)
    {
        var attr = new NullablePhoneAttribute();

        var result = attr.IsValid(value);

        Assert.True(result);
    }
}