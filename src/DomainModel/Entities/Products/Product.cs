using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Entities.Products
{
    public class Product : BaseEntity, IAggregateRoot
    {
        public int ProductId { get; protected set; }

        private readonly List<Option> _options = new List<Option>();
        public virtual IReadOnlyCollection<Option> Options => _options?.AsReadOnly();
    }
}
