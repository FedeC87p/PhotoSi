using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public List<int> OptionsId { get; set; }
    }
}
