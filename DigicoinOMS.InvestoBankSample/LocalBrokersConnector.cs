using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DigicoinOMS.Core.MarketConnectivity;
using DigicoinOMS.Core.Model;
using DigicoinOMS.InvestoBankSample.Broker;

namespace DigicoinOMS.InvestoBankSample
{
    class LocalBrokersConnector : IProcessableSubscriber<MarketQuote>, IQuoteRequestor, IProcessableSubscriber<Execution>, IExecutionRequestor
    {                
        private readonly object objectLock = new Object();

        // we're going to need these events since we implement two different flavours of IProcessableSubscriber<T>
        event ProcessableReceivedEventHandler<Execution> OnExecutionReceived; 
        event ProcessableReceivedEventHandler<MarketQuote> OnMarketQuoteReceived;

        // explicit interface event implementations
        event ProcessableReceivedEventHandler<Execution> IProcessableSubscriber<Execution>.OnProcessableItemReceived
        {
            add
            {
                lock (objectLock)
                {
                    OnExecutionReceived += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    OnExecutionReceived -= value;
                }
            }
        }

        event ProcessableReceivedEventHandler<MarketQuote> IProcessableSubscriber<MarketQuote>.OnProcessableItemReceived
        {
            add
            {
                lock (objectLock)
                {
                    OnMarketQuoteReceived += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    OnMarketQuoteReceived -= value;
                }
            }
        }
        
        public void RequestQuotes(Order order, List<int> quotesAmountsToRequest)
        {
            foreach (var quantity in quotesAmountsToRequest)
            {
                var marketQuote = new MarketQuote
                                      {
                                          OrderId = order.OrderId,
                                          Quotes = new List<IndividualBrokerQuote>()
                                      };

                foreach (var broker in this.brokers)
                {
                    marketQuote.Quotes.Add(broker.GetQuote(quantity));
                }

                Task.Factory.StartNew(
                    (Object arg) =>
                        {
                            var mquote = arg as MarketQuote;
                            var handler = OnMarketQuoteReceived;
                            if (handler != null)
                            {
                                handler(mquote);
                            }
                        },
                    marketQuote);
            }
        }


        

        public void ExecuteQuote(Order order, IndividualBrokerQuote quoteToExecute)
        {
            // publish execution
            var execToPublish = new Execution()
                                          {
                                              ExecutingBrokerName = quoteToExecute.BrokerName,
                                              OrderId = order.OrderId,
                                              Quantity = quoteToExecute.Quantity,
                                              TotalCost = quoteToExecute.TotalCost
                                          };
            Task.Factory.StartNew(
                (Object arg) =>
                {
                    var exec = arg as Execution;
                    var handler = OnExecutionReceived;
                    if (handler != null)
                    {
                        handler(exec);
                    }
                },
                execToPublish);
        }

        private readonly List<LocalBroker> brokers = new List<LocalBroker>(); 

        public LocalBrokersConnector()
        {
            brokers.Add(new LocalBroker("0", "Broker A", (decimal)1.49, new FixedRateCommissionCalculator()));
            brokers.Add(new LocalBroker("1,", "Broker B", (decimal)1.52, new SampleTieredCommissionCalculator()));
        }
        
    }
}
