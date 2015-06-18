using System;
using System.Collections.Generic;

using DigicoinOMS.Core.Components.Selectors;
using DigicoinOMS.Core.DataAccess;
using DigicoinOMS.Core.MarketConnectivity;
using DigicoinOMS.Core.Model;
using DigicoinOMS.Core.Utilities;

namespace DigicoinOMS.Core.Components
{
    public class QuoteProcessor : ProcessorBase<MarketQuote>
    {
        private IAuditLog AuditLog { get; set; }

        private IOrderStore OrderStore { get; set; }
        private IQuoteSelector QuoteSelector { get; set; }
        private IExecutionRequestor ExecutionRequestor { get; set; }

        public QuoteProcessor(IProcessableSubscriber<MarketQuote> quoteSubscriber, IOrderStore orderStore, IQuoteSelector quoteSelector, IExecutionRequestor executionRequestor, IAuditLog auditLog)
            : base(quoteSubscriber)
        {
            this.OrderStore = orderStore;
            this.QuoteSelector = quoteSelector;
            this.ExecutionRequestor = executionRequestor;
            this.AuditLog = auditLog;

            base.StartProcessing();
        }

        protected override void ProcessItem(MarketQuote quote)
        {
            var order = this.OrderStore.GetOrderById(quote.OrderId);

            try
            {
                if (order.ObtainedQuotes == null)
                {
                    order.ObtainedQuotes =  new List<IndividualBrokerQuote>();
                }
                order.ObtainedQuotes.AddRange(quote.Quotes);

                var individualQuoteToExecute = this.QuoteSelector.SelectQuote(order, quote.Quotes);

                this.ExecutionRequestor.ExecuteQuote(order, individualQuoteToExecute);
             
                order.State = OrderState.ExecutionRequested;
            }
            catch (Exception ex)
            {
                // log error
                order.State = OrderState.Failed;

            }
            finally
            {
                this.OrderStore.UpdateOrder(order);
            }
        }
    }
}
