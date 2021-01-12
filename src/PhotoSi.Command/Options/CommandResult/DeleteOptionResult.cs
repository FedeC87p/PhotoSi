using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSi.Command.Options.CommandResult
{
    public class DeleteOptionResult
    {
        public int OptionId { get; set; }
        public bool HaveError { get; set; }
        public List<string> Errors { get; set; }
    }
}
