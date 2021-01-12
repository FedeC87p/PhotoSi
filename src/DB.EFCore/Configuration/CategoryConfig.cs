using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.EFCore.Configuration
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.ToTable(TableNames.Category);

            entity.HasKey(p => p.CategoryId);

            entity.HasMany(p => p.Products).WithOne().HasForeignKey(p => p.CategoryId);

            entity.Ignore(b => b.DomainEvents);
            entity.Ignore(b => b.IntegrationEvents);
        }
    }
}
