using DomainModel.Dtos;
using DomainModel.Interfaces;
using DomainModel.Specifications.Rules;
using DomainModel.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities.Orders
{
    public class Order : BaseEntity, IAggregateRoot
    {
        public int OrderId { get; set; }
        public string Code { get; set; }
        public decimal Total { get; set; }


        private List<OrderItem> _orderItem;
        public virtual IReadOnlyCollection<OrderItem> OrderItems => _orderItem?.ToList();

        protected Order() { }

        public static async Task<IValidator<OrderDto, Order>> CreateOrderAsync(OrderDto orderDto)
        {
            var validator = new Validator<OrderDto, Order>(null);
            await validator.ExecuteCheckAsync(orderDto, new Order());

            if (!validator.IsValid)
            {
                return validator;
            }

            return null;
        }

        private void checkOrderValidity()
        {

        }

    }
}
