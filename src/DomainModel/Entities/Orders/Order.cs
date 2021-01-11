using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Entities.Orders
{
    public class Order : BaseEntity, IAggregateRoot
    {
        public int OrderId { get; set; }
    }
}
