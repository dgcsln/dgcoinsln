using System;
using System.Collections.Generic;

using Digicoin.DigicoinBank.Model;

namespace DigicoinOMS.Core.Model
{
    public class Order
    {
        public String OrderId { get; set; }

        public String ClientId { get; set; }

        public OrderState State { get; set; }

        public int Quantity { get; set; }

        public Side Side { get; set; }

        public Decimal TotalCost { get; set; }

        public List<IndividualBrokerQuote> ObtainedQuotes { get; set; }

        public List<Execution> Executions { get; set; }

        public int ExecutedQuantity { get; set; }

        public Decimal AverageExecutionPrice { get; set; }

        public String ErrorMessage { get; set; }
    }
}
