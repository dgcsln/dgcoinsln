using System.Linq;

using DigicoinOMS.Core.Components.QuoteCreation;
using DigicoinOMS.Core.Model;

using NUnit.Framework;

using Assert = NUnit.Framework.Assert;

namespace DigicoinOMS.Core.Tests.Components.QuoteCreation
{
    [TestFixture]
    public class BrokerQuantityRestrictedQuoteRequestCreatorTest
    {
        [Test]
        public void QuoteCreatorCreatesASingleRequestForQuantityUnderThreshold()
        {
            var underTest = new BrokerQuantityRestrictedQuoteRequestCreator();
            var testOrder = new Order { Quantity = 70 };
            var result = underTest.GetQuoteRequestAmounts(testOrder);

            Assert.AreEqual(1, result.Count);
            Assert.True(result.Contains(70));
        }

        [Test]
        public void QuoteCreatorCreatesASingleRequestForQuantityEqualToThreshold()
        {
            var underTest = new BrokerQuantityRestrictedQuoteRequestCreator();
            var testOrder = new Order { Quantity = 100 };
            var result = underTest.GetQuoteRequestAmounts(testOrder);

            Assert.AreEqual(1, result.Count);
            Assert.True(result.Contains(100));
        }

        [Test]
        public void QuoteCreatorCreatesMultipleRequestsForQuantityOverTheThreshold()
        {
            var underTest = new BrokerQuantityRestrictedQuoteRequestCreator();
            var testOrder = new Order { Quantity = 220 };
            var result = underTest.GetQuoteRequestAmounts(testOrder);

            // for a quantity of 220 we should get 3 quote amounts 100,100 and 20
            Assert.AreEqual(3, result.Count);
            Assert.True(result.Contains(20));
            Assert.AreEqual(2, result.Count(i => i == 100));
        }
    }
}
