using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.EFCore.Configuration
{

    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.ToTable(TableNames.Product);

            entity.HasKey(p => p.ProductId);

            entity.HasOne(p => p.Category).WithMany().OnDelete(DeleteBehavior.SetNull);
            entity.HasMany(p => p.Options).WithMany(p => p.Products);

            entity.Ignore(b => b.DomainEvents);
            entity.Ignore(b => b.IntegrationEvents);
        }
    }
}
