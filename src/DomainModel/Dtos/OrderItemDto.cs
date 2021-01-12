using DomainModel.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Dtos
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }

        public List<OrderItemOptionDto> OptionItems { get; set; }
    }
}
