using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities.Orders
{
    public class OrderItemOption
    {
        public int OrderItemOptionId { get; protected set; }
        public string Name { get; protected set; }
        public string Value { get; protected set; }
        public int OptionId { get; protected set; }

        public virtual OrderItem OrderItem { get; protected set; }
    }
}
