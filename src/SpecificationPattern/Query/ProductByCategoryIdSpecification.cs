using DomainModel.Entities.Products;
using DomainModel.Specifications.Query;
using System;

namespace SpecificationPattern.Query
{
    public class ProductByCategoryIdSpecification : BaseSpecification<Product>
    {
        public ProductByCategoryIdSpecification(int categoryId)
            : base(b => b.CategoryId == categoryId)
        {
        }
    }
}
