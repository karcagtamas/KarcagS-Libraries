using System;
using Karcags.Common.Enums;
using NUnit.Framework;

namespace Karcags.Common.Tests.Enums
{
    [TestFixture]
    public class OrderDirectionServiceTest
    {
        [Test]
        [TestCase(OrderDirection.Ascend, "asc")]
        [TestCase(OrderDirection.Descend, "desc")]
        [TestCase(OrderDirection.None, "none")]
        public void GetValue_GiveValidOrder_ReturnValidDirectionString(OrderDirection direction, string expectedResult)
        {
            var result = OrderDirectionService.GetValue(direction);
            
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetValue_GiveInvalidOrder_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => OrderDirectionService.GetValue((OrderDirection) 4));
        }
        
        [Test]
        [TestCase("asc", OrderDirection.Ascend)]
        [TestCase("desc", OrderDirection.Descend)]
        [TestCase("none", OrderDirection.None)]
        public void ValueToKey_GiveValidValue_ReturnValidDirection(string directionValue, OrderDirection expectedResult)
        {
            var result = OrderDirectionService.ValueToKey(directionValue);
            
            Assert.That(result, Is.EqualTo(expectedResult));
        }
        
        [Test]
        public void ValueToKey_GiveInvalidValue_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => OrderDirectionService.ValueToKey("alma"));
        }
    }
}