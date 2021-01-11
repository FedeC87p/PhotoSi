using DB.EFCore.Context;
using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.EFCore.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(DatabaseContext dbContext) : base(dbContext)
        {

        }
        public void LinkCategory(Product product, Category category)
        {
            product.SetCategory(category);
        }

        public void LinkOption(Product product, Option option)
        {
            product.AssignOption(option);
        }

        public void UnLinkCategory(Product product)
        {
            product.SetCategory(null);
        }

        public void UnLinkOption(Product product, Option option)
        {
            product.UnAssignOption(option);
        }
    }
}
