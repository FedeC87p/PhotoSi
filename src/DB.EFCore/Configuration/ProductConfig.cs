using DomainModel.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.EFCore.Configuration
{
    
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.HasKey(p => p.ProductId);

            entity.HasOne(p => p.Category).WithMany().OnDelete(DeleteBehavior.SetNull);

            entity.Ignore(b => b.DomainEvents);
        }
    }
}
