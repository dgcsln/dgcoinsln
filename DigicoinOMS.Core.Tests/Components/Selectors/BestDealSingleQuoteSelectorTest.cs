using System.Collections.Generic;

using DigicoinOMS.Core.Components.Selectors;
using DigicoinOMS.Core.Model;

using NUnit.Framework;

namespace DigicoinOMS.Core.Tests.Components.Selectors
{
    [TestFixture]
    public class BestDealSingleQuoteSelectorTest
    {
        [Test]
        public void SelectorChoosesLowestCostQuote()
        {
            var underTest = new BestDealSingleQuoteSelector();
            var first = new IndividualBrokerQuote { BrokerName = "Broker A", BrokerQuoteId = "0", Quantity = 70, TotalCost = (decimal)15.00 };
            var cheaper = new IndividualBrokerQuote { BrokerName = "Broker B", BrokerQuoteId = "0", Quantity = 70, TotalCost = (decimal)14.99 };
            var chosen = underTest.SelectQuote(new Order(), new List<IndividualBrokerQuote> { first, cheaper });

            Assert.AreEqual(cheaper, chosen);
        }

        [Test]
        public void SelectorChoosesAQuoteIfOnlyOneWasSupplied()
        {
            var underTest = new BestDealSingleQuoteSelector();
            var only = new IndividualBrokerQuote { BrokerName = "Broker A", BrokerQuoteId = "0", Quantity = 70, TotalCost = (decimal)15.00 };
            var chosen = underTest.SelectQuote(new Order(), new List<IndividualBrokerQuote> { only });

            Assert.AreEqual(only, chosen);
        }

        [Test]
        public void SelectorReturnsNullIfNoQuotesWereSupplied()
        {
            var underTest = new BestDealSingleQuoteSelector();
            var chosen = underTest.SelectQuote(new Order(), new List<IndividualBrokerQuote>());
            var chosen2 = underTest.SelectQuote(new Order(), null);

            Assert.AreEqual(null, chosen);
            Assert.AreEqual(null, chosen2);
        }
    }
}
