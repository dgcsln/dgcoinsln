using System;
using System.Collections.Generic;
using System.Linq;

using Digicoin.DigicoinBank.Model;

using DigicoinOMS.Core.DataAccess;
using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.Reporting
{
    public class DigiCoinReportGenerator
    {
        private IClientManager ClientManager { get; set; }
        private IOrderStore OrderStore { get; set; }

        public DigiCoinReportGenerator(IClientManager clientManager, IOrderStore orderStore)
        {
            this.ClientManager = clientManager;
            this.OrderStore = orderStore;
        }

        public String GetClientNetPositionsReport()
        {
            var summarisedOrders =
                this.OrderStore.GetOrders()
                    .GroupBy(o => new { o.ClientId })
                    .Select(
                        or =>
                        new ClientOrderSummary
                            {
                                ClientId = or.Key.ClientId.ToString(),
                                NetQuantity = or.Sum(ors => ors.Side == Side.Sell ? (-1 * ors.Quantity) : ors.Quantity),
                                AveragePrice = or.Average(ors => ors.AverageExecutionPrice)
                            });

            return String.Join(
                ", ",
                summarisedOrders.Select(
                    so => String.Format("{0} {1:0.000}", this.ClientManager.GetClientById(so.ClientId).ClientName, Decimal.Multiply(so.AveragePrice, so.NetQuantity))));
        }

        public String GetBrokerReport()
        {
            var brokerVolumes = new Dictionary<string, int>();

            foreach (Order or in this.OrderStore.GetOrders())
            {
                if (or.Executions == null || or.Executions.Count <= 0)
                {
                    continue;
                }

                foreach (var execution in or.Executions)
                {
                    var broker = execution.ExecutingBrokerName;
                    if (brokerVolumes.ContainsKey(broker))
                    {
                        brokerVolumes[broker] += execution.Quantity;
                    }
                    else
                    {
                        brokerVolumes[broker] = execution.Quantity;
                    }
                           
                }
            }
            return string.Join(", ", brokerVolumes.Select(kvp => string.Format("{0} {1}", kvp.Key, kvp.Value)));
        }

        private class ClientOrderSummary
        {
            public String ClientId { get; set; }
            public int NetQuantity { get; set; }
            public Decimal AveragePrice { get; set; }
        }
    }
}
