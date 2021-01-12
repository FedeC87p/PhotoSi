using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Dtos
{
    public class OptionDto
    {
        public int OptionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
    }
}
