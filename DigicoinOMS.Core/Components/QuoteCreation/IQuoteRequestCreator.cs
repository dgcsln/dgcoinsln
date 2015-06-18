using System.Collections.Generic;

using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.Components.QuoteCreation
{
    public interface IQuoteRequestCreator
    {
        List<int> GetQuoteRequestAmounts(Order order);
    }
}
