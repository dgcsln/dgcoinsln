using System.Collections.Generic;

using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.MarketConnectivity
{
    public interface IQuoteRequestor
    {
        void RequestQuotes(Order order, List<int> quotesAmountsToRequest);
    }
}
