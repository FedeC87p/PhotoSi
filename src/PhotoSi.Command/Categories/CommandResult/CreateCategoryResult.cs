using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSi.Command.Categories.CommandResult
{
    public class CreateCategoryResult
    {
        public int? CategoryId { get; set; }
        public bool HaveError { get; set; }
        public List<string> Errors { get; set; }
    }
}
