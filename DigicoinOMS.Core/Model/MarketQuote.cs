using System;
using System.Collections.Generic;

namespace DigicoinOMS.Core.Model
{
    public class MarketQuote : IProcessable
    {
        public String OrderId { get; set; }

        public List<IndividualBrokerQuote> Quotes { get; set; } 
    }
}
