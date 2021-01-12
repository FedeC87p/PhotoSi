using DomainModel.Entities.Products;
using DomainModel.Specifications.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationPattern.Query
{
    public class ProductByNameSpecification : BaseSpecification<Product>
    {
        public ProductByNameSpecification(string name, int excludeProductId)
            : base(b => b.Name == name && (excludeProductId == -1 || b.ProductId != excludeProductId))
        {
        }
    }
}
