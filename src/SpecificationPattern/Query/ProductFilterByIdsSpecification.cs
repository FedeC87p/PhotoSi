using DomainModel.Entities.Products;
using DomainModel.Specifications.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationPattern.Query
{
    public class ProductFilterByIdsSpecification : BaseSpecification<Product>
    {
        public ProductFilterByIdsSpecification(List<int> productIds)
            : base(b => productIds == null || productIds.Any(k => k == b.ProductId))
        {
        }
    }
}
