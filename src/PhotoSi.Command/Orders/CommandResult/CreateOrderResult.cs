using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSi.Command.Orders.CommandResult
{
    public class CreateOrderResult
    {
        public int? OrderId { get; set; }
        public bool HaveError { get; set; }
        public List<string> Errors { get; set; }
    }
}
