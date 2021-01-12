using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ModelViews.Request
{
    public class OptionCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
    }
}
