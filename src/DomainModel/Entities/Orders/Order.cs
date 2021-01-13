using DomainModel.Dtos;
using DomainModel.Events;
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


        private List<OrderItem> _orderItems = new List<OrderItem>();
        public virtual IReadOnlyCollection<OrderItem> OrderItems => _orderItems?.ToList();

        protected Order() { }

        public static async Task<IValidator<OrderDto, Order>> CreateOrderAsync(OrderDto orderDto, 
                                                                                IEnumerable<IRuleSpecification<OrderDto>> rules, 
                                                                                IEnumerable<IRuleSpecification<OrderItemDto>> orderItemRules,
                                                                                IEnumerable<IRuleSpecification<OrderItemOptionDto>> optionItemRules)
        {
            rules = rules ?? new List<IRuleSpecification<OrderDto>>();

            var validator = new Validator<OrderDto, Order>(rules);
            await validator.ExecuteCheckAsync(orderDto, new Order());

            if (!validator.IsValid)
            {
                return validator;
            }

            await createOrderItemAsync(orderDto, validator, orderItemRules, optionItemRules);

            validateCustomBusinessRulesOrderProducts(orderDto, validator);

            if (!validator.IsValid)
            {
                return validator;
            }

            validator.ValidatedObject.Code = orderDto.Code;

            validator.ValidatedObject.AddPublicEvent(new OrderConfirmedPublicEvent(validator.ValidatedObject));

            return validator;
        }

        static private void validateCustomBusinessRulesOrderProducts(OrderDto orderDto, Validator<OrderDto, Order> validator)
        {
            if (orderDto.ProductItems == null ||
                orderDto.ProductItems.Count <= 0)
            {
                var validatorError = new List<ValidatorError>();
                validatorError.Add(new ValidatorError
                {
                    Code = "NoProducts",
                    Detail = new ValidatorErrorDetail { Messages = new List<string> { "Non è stato selezionano nessun prodotto" } },
                    GeneratorClass = "OrderFactory",
                    Type = ValidatorType.Business
                });
                validator.AddCustomBrokenRule(validatorError);
            }
        }

        static private async Task createOrderItemAsync(OrderDto orderDto, Validator<OrderDto, Order> orderValidator,
                                                        IEnumerable<IRuleSpecification<OrderItemDto>> orderItemRules,
                                                        IEnumerable<IRuleSpecification<OrderItemOptionDto>> optionItemRules)
        {
            if (orderDto.ProductItems == null)
            {
                return;
            }

            foreach (var item in orderDto.ProductItems)
            {
                var validatorSingleItem = await OrderItem.CreateOrderItemAsync(item, orderItemRules, optionItemRules);

                if (!validatorSingleItem.IsValid)
                {
                    validatorSingleItem.BrokenRules?.ForEach(i => orderValidator.AddCustomBrokenRule(i));
                }
                else
                {
                    orderValidator.ValidatedObject._orderItems.Add(validatorSingleItem.ValidatedObject);
                }  
            }
        }

        //ORDINE TOTALMENTE IMMUTABLE
        //public void AddOrderItem(OrderItem orderItem)
        //{
        //    if (OrderItems == null)
        //    {
        //        _orderItems = new List<OrderItem>();
        //    }
        //    _orderItems.Add(orderItem);
        //}

        //public void RemoveOrderItem(OrderItem orderItem)
        //{
        //    if (OrderItems == null)
        //    {
        //        return;
        //    }
        //    _orderItems.Remove(orderItem);
        //}

    }
}
