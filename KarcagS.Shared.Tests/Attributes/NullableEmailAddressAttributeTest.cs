using KarcagS.Shared.Attributes;

namespace KarcagS.Shared.Tests.Attributes;

public class NullableEmailAddressAttributeTest
{
    [Fact]
    public void NullableEmailAddress_NullValue_ReturnTrue()
    {
        var attr = new NullableEmailAddressAttribute();

        var result = attr.IsValid(null);

        Assert.True(result);
    }

    [Fact]
    public void NullableEmailAddress_NotStringValue_ReturnFalse()
    {
        var attr = new NullableEmailAddressAttribute();

        var result = attr.IsValid(1);

        Assert.False(result);
    }

    [Theory]
    [InlineData("testemail")]
    [InlineData("testemail.hu")]
    public void NullableEmailAddress_InvalidEmailValue_ReturnFalse(string value)
    {
        var attr = new NullableEmailAddressAttribute();

        var result = attr.IsValid(value);

        Assert.False(result);
    }

    [Fact]
    public void NullableEmailAddress_ValidEmailValue_ReturnTrue()
    {
        var attr = new NullableEmailAddressAttribute();

        var result = attr.IsValid("test@email.hu");

        Assert.True(result);
    }
}