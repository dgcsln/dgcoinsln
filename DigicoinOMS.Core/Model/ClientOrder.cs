namespace DigicoinOMS.Core.Model
{
    public class ClientOrder : IProcessable
    {
        public int Quantity { get; set; }

        public string ClientName { get; set; }

        public string Side { get; set; }
    }
}
