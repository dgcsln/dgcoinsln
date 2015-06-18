namespace DigicoinOMS.Core.Api.Dto
{
    public class OrderRequest
    {
        public int Quantity { get; set; }

        public string ClientName { get; set; }

        public string Side { get; set; }
    }
}
