using DomainModel.Entities.Orders;
using DomainModel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.EFCore.Configuration
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity.ToTable(TableNames.Order);

            entity.HasKey(p => p.OrderId);

            entity.Ignore(b => b.DomainEvents);
            entity.Ignore(b => b.IntegrationEvents);
        }
    }
}
