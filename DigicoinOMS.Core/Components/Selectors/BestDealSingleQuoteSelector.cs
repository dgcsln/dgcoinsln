using System.Collections.Generic;
using System.Linq;

using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.Components.Selectors
{
    /// <summary>
    /// Simple quote selection strategy - choose a single quote to execute using the lowest cost as the only criterion.
    /// </summary> 
    public class BestDealSingleQuoteSelector : IQuoteSelector
    {
        public IndividualBrokerQuote SelectQuote(Order order, List<IndividualBrokerQuote> availableQuotes)
        {
            if (availableQuotes == null || !availableQuotes.Any())
            {
                return null;
            }

            return availableQuotes.Aggregate((lowest, current) => lowest.TotalCost < current.TotalCost ? lowest : current);

        }
    }
}
