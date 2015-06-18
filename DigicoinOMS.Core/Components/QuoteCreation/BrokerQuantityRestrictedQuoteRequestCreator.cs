using System.Collections.Generic;

using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.Components.QuoteCreation
{

    /// <summary>
    /// Quote request creation strategy to break down the order into chunks of maximum order size allowed per broker 
    /// and whatever is left.
    /// </summary>
    public class BrokerQuantityRestrictedQuoteRequestCreator : IQuoteRequestCreator
    {
        private const int MaxQuantityAllowedPerQuote = 100;

        public List<int> GetQuoteRequestAmounts(Order order)
        {
            var quoteAmounts = new List<int>();
            var remainingQuantity = order.Quantity;

            while (remainingQuantity > 0)
            {
                
                if (remainingQuantity > MaxQuantityAllowedPerQuote)
                {
                    quoteAmounts.Add(MaxQuantityAllowedPerQuote);
                    remainingQuantity -= MaxQuantityAllowedPerQuote;
                }
                else
                {
                    quoteAmounts.Add(remainingQuantity);
                    break;
                }
            }

            return quoteAmounts;
        }
    }
}
