using DomainModel.Dtos;
using DomainModel.Interfaces;
using DomainModel.Specifications.Rules;
using DomainModel.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Entities.Orders
{
    public class Order : BaseEntity, IAggregateRoot
    {
        public int OrderId { get; set; }
        public string Code { get; set; }
        public decimal Total { get; set; }


        private List<OrderItem> _orderItems;
        public virtual IReadOnlyCollection<OrderItem> OrderItems => _orderItems?.ToList();

        protected Order() { }

        public static async Task<IValidator<OrderDto, Order>> CreateOrderAsync(OrderDto orderDto)
        {
            var rules = new List<IRuleSpecification<OrderDto>>();

            var validator = new Validator<OrderDto, Order>(rules);
            await validator.ExecuteCheckAsync(orderDto, new Order());

            await createOrderItemAsync(orderDto, validator);

            validateCustomBusinessRulesOrderProducts(orderDto, validator);

            if (!validator.IsValid)
            {
                return validator;
            }

            return null;
        }

        static private void validateCustomBusinessRulesOrderProducts(OrderDto orderDto, Validator<OrderDto, Order> validator)
        {
            var validatorError = new List<ValidatorError>();

            if (orderDto.ProductItems == null ||
                orderDto.ProductItems.Count <= 0)
            {
                validatorError.Add(new ValidatorError
                {
                    Code = "NoProducts",
                    Detail = new ValidatorErrorDetail { Messages = new List<string> { "Non è stato selezionano nessun prodotto" } },
                    GeneratorClass = "OrderFactory",
                    Type = ValidatorType.Business
                });
            }
        }

        static private async Task createOrderItemAsync(OrderDto orderDto, Validator<OrderDto, Order> validator)
        {
            List<string> errors = null;
            foreach (var item in orderDto.ProductItems)
            {
                var validatorProduct = await OrderItem.CreateOrderItemAsync(item);

                if (!validatorProduct.IsValid)
                {
                    errors = validatorProduct.BrokenRules.SelectMany(i => i.Errors.Select(k => k.Code)).ToList();
                }
            }
            if (errors != null &&
                errors.Count > 0)
            {
                validator.AddCustomBrokenRule(errors.Select(i => new ValidatorError
                {
                    Code = "InvalidProductItem",
                    Detail = new ValidatorErrorDetail { Messages = new List<string> { i } },
                    GeneratorClass = "OrderFactory",
                    Type = ValidatorType.DomainEntity
                }).ToList()
                );
            }
        }

    }
}
