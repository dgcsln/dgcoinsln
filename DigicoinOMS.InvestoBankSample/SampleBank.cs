using DigicoinOMS.Core.Components;
using DigicoinOMS.Core.Components.QuoteCreation;
using DigicoinOMS.Core.Components.Selectors;
using DigicoinOMS.Core.Components.Validation;
using DigicoinOMS.Core.DataAccess;
using DigicoinOMS.Core.Reporting;
using DigicoinOMS.Core.Utilities;

namespace DigicoinOMS.InvestoBankSample
{
    /// <summary>
    /// This class creates an instance of Digicoin trading app with a local client and broker interfaces for testing purposes.
    /// If we were to use a 3rd party DI library such as ninject, this could be dealt with using configuration. 
    /// Think of the SampleBank and the wrapping console app as a test harness for the logic in OMS.Core.
    /// 
    /// The idea is that the OMS.Core is just a collection of framework components and business logic and what the exact app instance does
    /// is decided by how you configure and instantiate it. Processors in the OMS.Core are completely agnostic as to where the messages
    /// are coming from - in this test app we generated them in-process, but to swap it out for, let's say a webservice handler is simply
    /// a matter of injecting a different implementation of the relevant subscriber interface at startup. Same goes for Requestors/Publishers and
    /// the Data managers - in the sample we use in-memory implementations, but these can easily be swapped out for something backed by a DB. 
    /// </summary>
    class SampleBank
    {
        public LocalClientConnector ClientInterface { get; set; }

        public DigiCoinReportGenerator ReportInterface { get; set; }

        public IAuditLogReader AuditLogReader { get; set; }

        public void Initialise()
        {
            var auditLog = new InMemoryAuditLog();
            this.AuditLogReader = auditLog;

            IOrderStore orderStore = new InMemoryOrderStore();
            this.ClientInterface = new LocalClientConnector();

            var localBrokers = new LocalBrokersConnector();

            IClientManager clientManager = new InMemoryClientManager();
            clientManager.RegisterClient("Client A");
            clientManager.RegisterClient("Client B");
            clientManager.RegisterClient("Client C");



            var orderProcessor = new ClientOrderProcessor(
                this.ClientInterface, orderStore, new BasicOrderValidator(), clientManager, 
                new BrokerQuantityRestrictedQuoteRequestCreator(),
                localBrokers, auditLog);

            var orderMonitor = new OrderStateMonitor(orderStore, this.ClientInterface, clientManager);

            var quoteProcessor = new QuoteProcessor(localBrokers, orderStore, new BestDealSingleQuoteSelector(), localBrokers, auditLog);

            var executionProcessor = new ExecutionProcessor(localBrokers, orderStore);

            this.ReportInterface = new DigiCoinReportGenerator(clientManager, orderStore);
        }
    }
}
