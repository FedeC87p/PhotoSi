using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSi.Interfaces.Configuration
{
    public class EntityConfig
    {
        public class ValidationRulesConfig
        {
            public List<string> Product { get; set; }
            public List<string> Category { get; set; }
            public List<string> Option { get; set; }
        }
    }
}
