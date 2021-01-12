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
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity.ToTable(TableNames.OrderItem);

            entity.HasKey(p => p.OrderItemId);

            entity.HasOne(p => p.Order).WithMany(p => p.OrderItems).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
