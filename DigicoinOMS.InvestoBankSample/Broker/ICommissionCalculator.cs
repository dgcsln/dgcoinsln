using System;

namespace DigicoinOMS.InvestoBankSample.Broker
{
    interface ICommissionCalculator
    {
        Decimal GetCommissionRate(int quantity);
    }
}
