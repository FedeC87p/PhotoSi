using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Entities.Products
{
    public class Option : BaseEntity, IAggregateRoot
    {
        public int OptionId { get; protected set; }

        
    }
}
