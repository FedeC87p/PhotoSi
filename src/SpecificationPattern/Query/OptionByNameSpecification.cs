using DomainModel.Entities.Products;
using DomainModel.Specifications.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationPattern.Query
{
    public class OptionByNameSpecification : BaseSpecification<Option>
    {
        public OptionByNameSpecification(string name, int excludeOptionId)
            : base(b => b.Name == name && (excludeOptionId == -1 || b.OptionId != excludeOptionId))
        {
        }
    }
}
