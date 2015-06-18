using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.DataAccess
{
    public class InMemoryOrderStore : IOrderStore
    {
        private ConcurrentDictionary<String, Order> orders = new ConcurrentDictionary<string, Order>();

        private static int orderIdCounter = 0;

        public event OrderStatusUpdatedHandler OnOrderStatusUpdated;

        public Order GetOrderById(string orderId)
        {
            Order ret;
            this.orders.TryGetValue(orderId, out ret);
            return ret;
        }

        public Order UpdateOrder(Order orderToUpdate)
        {
            if (orderToUpdate.State == OrderState.Filled || orderToUpdate.State == OrderState.Failed)
            {
                Task.Factory.StartNew(
                    (Object arg) =>
                    {
                        var ord = arg as Order;
                        OrderStatusUpdatedHandler handler = OnOrderStatusUpdated;
                        if (handler != null)
                        {
                            handler(ord);
                        }
                    },
                    orderToUpdate);
            }
            
            return orderToUpdate;
        }

        public Order AddOrder(Order orderToAdd)
        {
            var orderId = Interlocked.Increment(ref orderIdCounter).ToString();
            orderToAdd.OrderId = orderId;
            this.orders.TryAdd(orderId, orderToAdd);
            return orderToAdd;
        }

        public IEnumerable<Order> GetOrders()
        {
            return this.orders.Values;
        }
    }
}
