using Digicoin.DigicoinBank.Model;

using DigicoinOMS.Core.Api.Dto;
using DigicoinOMS.Core.DataAccess;
using DigicoinOMS.Core.MarketConnectivity;
using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.Components
{
    public class OrderStateMonitor
    {
        private IOrderStatusPublisher StatusPublisher { get; set; }

        private IOrderStore OrderStore { get; set; }

        private IClientManager ClientManager { get; set; }

        public OrderStateMonitor(IOrderStore orderStore, IOrderStatusPublisher statusPublisher, IClientManager clientManager)
        {
            this.OrderStore = orderStore;
            this.StatusPublisher = statusPublisher;
            this.ClientManager = clientManager;

            this.OrderStore.OnOrderStatusUpdated += this.OrderStore_OnOrderStatusUpdated;
        }

        void OrderStore_OnOrderStatusUpdated(Model.Order order)
        {
            if (order.State == OrderState.Filled)
            {
                this.StatusPublisher.PublishOrderStatus(new OrderStatus
                                                       {
                                                           StatusMessage = string.Format("{0} {1} {2} at {3}",
                                                           this.ClientManager.GetClientById(order.ClientId).ClientName,
                                                           order.Side == Side.Buy ? "buys" : "sells",
                                                           order.Quantity,
                                                           order.TotalCost)
                                                       });
            }

            if (order.State == OrderState.Failed)
            {
                var errorMessage = "An error has occurred while processing the order.";
                if (order.ErrorMessage != null)
                {
                    errorMessage += (" " + order.ErrorMessage);
                }
                this.StatusPublisher.PublishOrderStatus(new OrderStatus{StatusMessage = errorMessage});
            }
        }
    }
}
