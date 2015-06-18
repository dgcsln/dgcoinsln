namespace DigicoinOMS.InvestoBankSample.Broker
{
    class SampleTieredCommissionCalculator : ICommissionCalculator
    {
        public decimal GetCommissionRate(int quantity)
        {
            if (quantity < 50)
            {
                return (decimal)0.03;
            }
            if (quantity < 90)
            {
                return (decimal)0.025;
            }
            return (decimal)0.02;
        }
    }
}
