using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string Code { get; set; }
        public decimal Total { get; set; }

        public List<OrderItemDto> ProductItems { get; set; }
    }
}
