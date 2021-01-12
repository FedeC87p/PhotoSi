using DomainModel.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Events
{
    public class OrderConfirmedPublicEvent : PublicEventBase
    {
        public Order Order { get; }

        public OrderConfirmedPublicEvent(Order order)
        {
            Order = order;
        }

    }
}
