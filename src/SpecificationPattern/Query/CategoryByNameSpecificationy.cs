using DomainModel.Entities.Products;
using DomainModel.Specifications.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationPattern.Query
{
    public class CategoryByNameSpecification : BaseSpecification<Category>
    {
        public CategoryByNameSpecification(string name, int excludeCategoryId)
            : base(b => b.Name == name && (excludeCategoryId == -1 || b.CategoryId != excludeCategoryId))
        {
        }
    }
}
