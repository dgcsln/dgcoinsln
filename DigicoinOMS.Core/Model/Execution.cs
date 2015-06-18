using System;

namespace DigicoinOMS.Core.Model
{
    public class Execution : IProcessable
    {
        public String ExecutingBrokerName { get; set; }

        public String OrderId { get; set; }

        public int Quantity { get; set; }

        public Decimal TotalCost { get; set; }
    }
}
