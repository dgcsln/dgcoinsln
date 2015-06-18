namespace DigicoinOMS.Core.Model
{
    public class IndividualBrokerQuote
    {
        public string BrokerName { get; set; }

        public string BrokerQuoteId { get; set; }

        public int Quantity { get; set; }

        public decimal TotalCost { get; set; }

    }
}
