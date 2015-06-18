using System;
using System.Collections.Generic;

using DigicoinOMS.Core.DataAccess;
using DigicoinOMS.Core.MarketConnectivity;
using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.Components
{
    public class ExecutionProcessor : ProcessorBase<Execution>
    {
        private IOrderStore OrderStore { get; set; }

        public ExecutionProcessor(IProcessableSubscriber<Execution> executionSubscriber, IOrderStore orderStore) : base(executionSubscriber)
        {
            this.OrderStore = orderStore;
            base.StartProcessing();
        }

        protected override void ProcessItem(Execution execution)
        {
            // pickup the order from store
            var order = this.OrderStore.GetOrderById(execution.OrderId);
            try
            {
                // validate quantity
                if (order.ExecutedQuantity + execution.Quantity <= order.Quantity)
                {
                    order.ExecutedQuantity += execution.Quantity;
                    order.TotalCost += execution.TotalCost;

                    // set fill state
                    order.State = order.ExecutedQuantity < order.Quantity
                                      ? OrderState.PartiallyFilled
                                      : OrderState.Filled;

                    order.AverageExecutionPrice = Decimal.Divide(order.TotalCost, order.ExecutedQuantity);

                    if (order.Executions == null)
                    {
                        order.Executions = new List<Execution>();
                    }

                    order.Executions.Add(execution);

                    //audit log, processed execution
                }
                else
                {
                    order.State = OrderState.Failed;
                    // audit log - cannot process execution for order id. Executed Quantity exceeded total Order Quantity.
                }
            }
            catch (Exception ex)
            {
                order.State = OrderState.Failed;
            }
            finally
            {
                this.OrderStore.UpdateOrder(order);
            }
        }      
    }
}
