using DomainModel.Dtos;
using DomainModel.Specifications.Rules;
using DomainModel.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainModel.Entities.Orders
{
    public class OrderItemOption
    {
        public int OrderItemOptionId { get; protected set; }
        public string Name { get; protected set; }
        public string Value { get; protected set; }
        public int OptionId { get; protected set; }

        public int OrderItemId { get; protected set; }
        public virtual OrderItem OrderItem { get; protected set; }

        public static async Task<IValidator<OrderItemOptionDto, OrderItemOption>> CreateOptionItemAsync(OrderItemOptionDto orderItemOptionDto,
                                                                                                        IEnumerable<IRuleSpecification<OrderItemOptionDto>> rules)
        {
            rules = rules ?? new List<IRuleSpecification<OrderItemOptionDto>>();
            var validator = new Validator<OrderItemOptionDto, OrderItemOption>(rules);
            await validator.ExecuteCheckAsync(orderItemOptionDto, new OrderItemOption());

            if (!validator.IsValid)
            {
                return validator;
            }

            validator.ValidatedObject.Name = orderItemOptionDto.Name;
            validator.ValidatedObject.Value = orderItemOptionDto.Value;
            validator.ValidatedObject.OptionId = orderItemOptionDto.OptionId;

            return validator;
        }
    }
}
