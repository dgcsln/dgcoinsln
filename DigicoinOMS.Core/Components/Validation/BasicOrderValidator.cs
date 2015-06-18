using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.Components.Validation
{
    /// <summary>
    /// Simple Validator to check if the order quantity is greater than 0 and a multiple of 10 and whether the order is for a valid client.
    /// </summary>
    public class BasicOrderValidator : IOrderValidator
    {
        public void ValidateOrder(Order order)
        {
            if (order.ClientId == null)
            {
                order.State = OrderState.Failed;
                order.ErrorMessage = "Unknown Client specified.";
                return;
            }
            if (order.Quantity <= 0)
            {
                order.State = OrderState.Failed;
                order.ErrorMessage = string.Format("Order quantity must be greater than 0. Received quantity of {0}", order.Quantity);
                return;
            }

            if (order.Quantity % 10 == 0)
            {
                return;
            }

            order.State = OrderState.Failed;
            order.ErrorMessage = string.Format("Order quantity of {0}, is not a multiple of 10", order.Quantity);
        }
    }
}
