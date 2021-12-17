using Karcags.Common.Annotations;
using NUnit.Framework;

namespace Karcags.Common.Tests.Annotations
{
    [TestFixture]
    public class MinNumberAttributeTest
    {
        [Test]
        [TestCase(10, 6, true)]
        [TestCase(7, 5, true)]
        [TestCase(12, -3, true)]
        [TestCase(44,44, true)]
        [TestCase(12, 13, false)]
        [TestCase(2,9, false)]
        public void MinNumber_ValidNumbers_AreValid(int number, int min, bool expectedResult)
        {
            var attr = new MinNumberAttribute(min);

            var result = attr.IsValid(number);
            
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}