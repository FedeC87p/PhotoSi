using DomainModel.Dtos;
using DomainModel.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Entities.Orders
{
    public class OrderItem
    {
        public int OrderItemId { get; protected set; }
        public string Name { get; protected set; }
        public int Quantity { get; protected set; }
        public decimal UnitPrice { get; protected set; }
        public int ProductId { get; protected set; }

        public int OrderId { get; protected set; }
        public virtual Order Order { get; protected set; }

        private List<OrderItemOption> _itemOptions;
        public virtual IReadOnlyCollection<OrderItemOption> ItemOptions => _itemOptions?.ToList();

        protected OrderItem() { }

        public static async Task<IValidator<OrderItemDto, OrderItem>> CreateOrderItemAsync(OrderItemDto orderItemDto)
        {
            var validator = new Validator<OrderItemDto, OrderItem>(null);
            await validator.ExecuteCheckAsync(orderItemDto, new OrderItem());

            await createOptionItemAsync(orderItemDto, validator);

            if (!validator.IsValid)
            {
                return validator;
            }

            validator.ValidatedObject.Name = orderItemDto.Name;
            validator.ValidatedObject.Quantity = orderItemDto.Quantity;
            validator.ValidatedObject.UnitPrice = orderItemDto.UnitPrice;
            validator.ValidatedObject.ProductId = orderItemDto.ProductId;

            return validator;
        }

        static private async Task createOptionItemAsync(OrderItemDto orderItemDto, Validator<OrderItemDto, OrderItem> validator)
        {
            List<string> errors = null;
            foreach (var item in orderItemDto.OptionItems)
            {
                var validatorOption = await OrderItemOption.CreateOptionItemAsync(item);

                if (!validatorOption.IsValid)
                {
                    errors = validatorOption.BrokenRules.SelectMany(i => i.Errors.Select(k => k.Code)).ToList();
                }
            }
            if (errors != null &&
                errors.Count > 0)
            {
                validator.AddCustomBrokenRule(errors.Select(i => new ValidatorError
                {
                    Code = "InvalidOrderItem",
                    Detail = new ValidatorErrorDetail { Messages = new List<string> { i } },
                    GeneratorClass = "OrderItemFactory",
                    Type = ValidatorType.DomainEntity
                }).ToList()
                );
            }
        }
    }
}
