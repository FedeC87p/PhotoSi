using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ModelViews.Request
{
    public class OrderCreateRequest
    {
        public string Code { get; set; }
        public List<ProductItem> ProductItems { get; set; }

        public class ProductItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public List<OptionItem> OptionItems { get; set; }
        }

        public class OptionItem
        {
            public int OptionId { get; set; }
            public string Value { get; set; }
        }
    }
}
