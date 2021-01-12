using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities.Orders
{
    public class OrderItem
    {
        public int OrderItemId { get; protected set; }
        public string Name { get; protected set; }
        public int Quantity { get; protected set; }
        public decimal UnitPrice { get; protected set; }
        public int ProductId { get; protected set; }


        public virtual Order Order { get; protected set; }

        private List<OrderItemOption> _optionItem;
        public virtual IReadOnlyCollection<OrderItemOption> OptionItems => _optionItem?.ToList();

        protected OrderItem() { }
    }
}
