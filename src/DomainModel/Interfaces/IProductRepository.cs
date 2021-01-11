using DomainModel.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void LinkCategory(Product product, Category category);
        void LinkOption(Product product, Option option);
        void UnLinkCategory(Product product);
        void UnLinkOption(Product product, Option option);
    }
}
