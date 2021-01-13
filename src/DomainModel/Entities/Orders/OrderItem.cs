using DomainModel.Dtos;
using DomainModel.Specifications.Rules;
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
        public int ProductId { get; protected set; }

        public int OrderId { get; protected set; }
        public virtual Order Order { get; protected set; }

        private List<OrderItemOption> _itemOptions = new List<OrderItemOption>();
        public virtual IReadOnlyCollection<OrderItemOption> ItemOptions => _itemOptions?.ToList();

        protected OrderItem() { }

        public static async Task<IValidator<OrderItemDto, OrderItem>> CreateOrderItemAsync(OrderItemDto orderItemDto,
                                                                                            IEnumerable<IRuleSpecification<OrderItemDto>> rules,
                                                                                            IEnumerable<IRuleSpecification<OrderItemOptionDto>> optionItemRules)
        {
            rules = rules ?? new List<IRuleSpecification<OrderItemDto>>();
            var validator = new Validator<OrderItemDto, OrderItem>(rules);
            await validator.ExecuteCheckAsync(orderItemDto, new OrderItem());

            if (!validator.IsValid) //Troppo ripetitivi andrebbe fatto un refactory
            {
                return validator;
            }

            await createOptionItemAsync(orderItemDto, validator, optionItemRules);

            if (!validator.IsValid) //Troppo ripetitivi andrebbe fatto un refactory
            {
                return validator;
            }

            validator.ValidatedObject.Name = orderItemDto.Name;
            validator.ValidatedObject.Quantity = orderItemDto.Quantity;
            validator.ValidatedObject.ProductId = orderItemDto.ProductId;

            return validator;
        }

        static private async Task createOptionItemAsync(OrderItemDto orderItemDto, Validator<OrderItemDto, OrderItem> validatorItem,
                                                        IEnumerable<IRuleSpecification<OrderItemOptionDto>> optionItemRules)
        {
            if (orderItemDto.OptionItems == null)
            {
                return;
            }

            foreach (var item in orderItemDto.OptionItems)
            {
                var validatorOption = await OrderItemOption.CreateOptionItemAsync(item, optionItemRules);

                if (!validatorOption.IsValid)
                {
                    validatorOption.BrokenRules?.ForEach(i => validatorItem.AddCustomBrokenRule(i));
                }
                else
                {
                    validatorItem.ValidatedObject._itemOptions.Add(validatorOption.ValidatedObject);
                }
            }
        }
    }
}
