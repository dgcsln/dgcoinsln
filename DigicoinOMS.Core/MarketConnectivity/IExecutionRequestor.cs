using DigicoinOMS.Core.Model;

namespace DigicoinOMS.Core.MarketConnectivity
{
    public interface IExecutionRequestor
    {
        void ExecuteQuote(Order order, IndividualBrokerQuote quoteToExecute);
    }
}
