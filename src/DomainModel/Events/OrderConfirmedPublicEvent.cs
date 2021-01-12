using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Events
{
    public class OrderConfirmedPublicEvent : PublicEventBase
    {
        public int OrderId { get; }

        public OrderConfirmedPublicEvent(int orderId)
        {
            OrderId = OrderId;
        }

    }
}
