using System;

using DigicoinOMS.Core.Model;

namespace DigicoinOMS.InvestoBankSample.Broker
{
    class LocalBroker
    {
        private String BrokerId { get; set; }

        private Decimal Price { get; set; }

        private ICommissionCalculator CommissionCalculator { get; set; }

        private String BrokerName { get; set; }

        public LocalBroker(String brokerId, String name, Decimal price, ICommissionCalculator commissionCalculator)
        {
            this.BrokerId = brokerId;
            this.BrokerName = name;
            this.Price = price;
            this.CommissionCalculator = commissionCalculator;
        }

        public IndividualBrokerQuote GetQuote(int quantity)
        {
            var grossProceeds = Decimal.Multiply(Price, quantity);
            var commissionFactor = Decimal.Add(1, this.CommissionCalculator.GetCommissionRate(quantity));

            return new IndividualBrokerQuote
                       {
                           BrokerName = this.BrokerName,
                           Quantity = quantity,
                           TotalCost = Decimal.Multiply(grossProceeds, commissionFactor)
                       };
        }
    }
}
