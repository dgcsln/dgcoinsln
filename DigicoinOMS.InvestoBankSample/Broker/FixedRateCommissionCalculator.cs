using System;

namespace DigicoinOMS.InvestoBankSample.Broker
{
    class FixedRateCommissionCalculator : ICommissionCalculator
    {
        private const Decimal FixedCommissionRate = (decimal)0.05;

        public decimal GetCommissionRate(int quantity)
        {
            return FixedCommissionRate;
        }
    }
}
