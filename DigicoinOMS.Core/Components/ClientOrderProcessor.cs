using System;

using Digicoin.DigicoinBank.Model;

using DigicoinOMS.Core.Components.QuoteCreation;
using DigicoinOMS.Core.Components.Validation;
using DigicoinOMS.Core.DataAccess;
using DigicoinOMS.Core.MarketConnectivity;
using DigicoinOMS.Core.Model;
using DigicoinOMS.Core.Utilities;

namespace DigicoinOMS.Core.Components
{
    public class ClientOrderProcessor : ProcessorBase<ClientOrder>
    {
        private IAuditLog AuditLog { get; set; }
        private IOrderValidator OrderValidator { get; set; }
        private IOrderStore OrderStore { get; set; }
        private IClientManager ClientManager { get; set; }
        private IQuoteRequestCreator OrderQuoteRequestCreator { get; set; }
        private IQuoteRequestor QuoteRequestor { get; set; }

        public ClientOrderProcessor(IProcessableSubscriber<ClientOrder> clientOrderSubscriber, IOrderStore orderStore, IOrderValidator orderValidator,
            IClientManager clientManager, IQuoteRequestCreator quoteCreator, IQuoteRequestor quoteRequestor, IAuditLog auditLog)
            : base(clientOrderSubscriber)
        {
            this.OrderStore = orderStore;
            this.OrderValidator = orderValidator;
            this.ClientManager = clientManager;
            this.OrderQuoteRequestCreator = quoteCreator;
            this.QuoteRequestor = quoteRequestor;
            this.AuditLog = auditLog;

            base.StartProcessing();
        }

        protected override void ProcessItem(ClientOrder clientOrder)
        {
            var savedOrder = new Order();
            try
            {
                if (clientOrder == null)
                {
                    AuditLog.Log("Received an null Client Order.");
                    return;
                }

                var client = this.ClientManager.GetClientByName(clientOrder.ClientName);

                var order = new Order
                                {
                                    ClientId = client != null ? client.ClientId : null,
                                    Quantity = clientOrder.Quantity,
                                    Side = (Side)Enum.Parse(typeof(Side), clientOrder.Side, true),
                                    State = OrderState.Received                                  
                                };

                this.OrderValidator.ValidateOrder(order);
                savedOrder = this.OrderStore.AddOrder(order);

                this.AuditLog.Log(
                    string.Format(
                        "Processing client order: Id={0}, Client = {1}, Quantity = {2}, Side = {3}",
                        savedOrder.OrderId,
                        clientOrder.ClientName,
                        savedOrder.Quantity,
                        savedOrder.Side));

               
            }
            catch
            {
                savedOrder.State = OrderState.Failed;
                
                // if we managed to save the order, trigger update
                if (savedOrder.OrderId != null)
                {
                    this.OrderStore.UpdateOrder(savedOrder);
                }
                
                return;
            }

            if (savedOrder.State != OrderState.Received)
            {
                this.AuditLog.Log(string.Format("Error processing order with Id={0}. Error message={1}", savedOrder.OrderId, savedOrder.ErrorMessage));
                this.OrderStore.UpdateOrder(savedOrder);
                return;
            }

            try
            {
                // request quotes
                var quotesToRequest = this.OrderQuoteRequestCreator.GetQuoteRequestAmounts(savedOrder);
                this.QuoteRequestor.RequestQuotes(savedOrder, quotesToRequest);

                savedOrder.State = OrderState.Accepted;
            }
            catch (Exception ex)
            {
                //log error
                // audit log
                savedOrder.State = OrderState.Failed;
            }
            finally
            {
                this.OrderStore.UpdateOrder(savedOrder);
            }
        }
    }

}
