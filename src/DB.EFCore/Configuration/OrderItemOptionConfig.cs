using DomainModel.Entities.Orders;
using DomainModel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.EFCore.Configuration
{
    public class OrderItemOptionConfig : IEntityTypeConfiguration<OrderItemOption>
    {
        public void Configure(EntityTypeBuilder<OrderItemOption> entity)
        {
            entity.ToTable(TableNames.OrderItemOption);

            entity.HasKey("OrderItemOptionId", "OrderItemId");

            entity.HasOne(p => p.OrderItem).WithMany(p => p.ItemOptions).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
