using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSi.Command.Product.CommandResult
{
    public class UpdateProductResult
    {
        public int? ProductId { get; set; }
        public bool HaveError { get; set; }
        public List<string> Errors { get; set; }
    }
}
