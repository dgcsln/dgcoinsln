using System;
using System.Collections.Generic;

using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.DataAccess
{
    public delegate void OrderStatusUpdatedHandler(Order order);

    public interface IOrderStore
    {
        event OrderStatusUpdatedHandler OnOrderStatusUpdated;

        Order GetOrderById(String orderId);

        Order UpdateOrder(Order orderToUpdate);

        Order AddOrder(Order orderToAdd);

        IEnumerable<Order> GetOrders();
    }
}
