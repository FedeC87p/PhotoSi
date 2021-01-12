using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Dtos
{
    public class OrderItemOptionDto
    {
        public int OrderItemOptionId { get; set; }
        public int OrderItemId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int OptionId { get; set; }
    }
}
