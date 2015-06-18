using System.Collections.Generic;

using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.Components.Selectors
{
    public interface IQuoteSelector
    {
        IndividualBrokerQuote SelectQuote(Order order, List<IndividualBrokerQuote> availableQuotes);
    }
}
