using KarcagS.Common.Annotations;
using NUnit.Framework;

namespace KarcagS.Common.Tests.Annotations
{
    [TestFixture]
    public class MaxNumberAttributeTest
    {
        [Test]
        [TestCase(10, 12, true)]
        [TestCase(2, 5, true)]
        [TestCase(-32, 12, true)]
        [TestCase(23,23, true)]
        [TestCase(43, 12, false)]
        [TestCase(2,1, false)]
        public void MaxNumber_ValidNumbers_AreValid(int number, int max, bool expectedResult)
        {
            var attr = new MaxNumberAttribute(max);

            var result = attr.IsValid(number);
            
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}