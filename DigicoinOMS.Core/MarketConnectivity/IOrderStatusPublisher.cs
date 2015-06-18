using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrderStatus = DigicoinOMS.Core.Api.Dto.OrderStatus;

namespace DigicoinOMS.Core.MarketConnectivity
{
    public interface IOrderStatusPublisher
    {
        void PublishOrderStatus(OrderStatus status);
    }
}
