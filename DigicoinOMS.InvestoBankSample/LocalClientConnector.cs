using System;
using System.Threading.Tasks;

using DigicoinOMS.Core.Api.Dto;
using DigicoinOMS.Core.MarketConnectivity;
using DigicoinOMS.Core.Model;

namespace DigicoinOMS.InvestoBankSample
{
    /// <summary>
    /// Simple implementation of the Client communication interfaces.
    /// </summary>
    class LocalClientConnector : IProcessableSubscriber<ClientOrder>, IOrderStatusPublisher
    {

        public void ProcessClientOrder(OrderRequest clientOrderRequest)
        {
            var clientOrder = new ClientOrder
                                  {
                                      ClientName = clientOrderRequest.ClientName,
                                      Quantity = clientOrderRequest.Quantity,
                                      Side = clientOrderRequest.Side
                                  };
            Task.Factory.StartNew(
                (Object arg) =>
                    {
                        var clo = arg as ClientOrder;
                        var handler = OnProcessableItemReceived;
                        if (handler != null)
                        {
                            handler(clo);
                        }
                    },
                clientOrder);
        }

        public void PublishOrderStatus(OrderStatus status)
        {
            Console.WriteLine(status.StatusMessage);
        }

        public event ProcessableReceivedEventHandler<ClientOrder> OnProcessableItemReceived;
    }
}
