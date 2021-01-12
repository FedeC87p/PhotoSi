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


        private List<OrderItem> _orderItems = new List<OrderItem>();
        public virtual IReadOnlyCollection<OrderItem> OrderItems => _orderItems?.ToList();

        protected Order() { }

        public static async Task<IValidator<OrderDto, Order>> CreateOrderAsync(OrderDto orderDto, List<IRuleSpecification<OrderDto>> rules)
        {
            rules = rules ?? new List<IRuleSpecification<OrderDto>>();

            var validator = new Validator<OrderDto, Order>(rules);
            await validator.ExecuteCheckAsync(orderDto, new Order());

            await createOrderItemAsync(orderDto, validator);

            validateCustomBusinessRulesOrderProducts(orderDto, validator);

            if (!validator.IsValid)
            {
                return validator;
            }

            validator.ValidatedObject.Code = orderDto.Code;
            validator.ValidatedObject.Total = orderDto.Total; //In teoria dovrei calcolarlo in case ai prodotti figli
            validator.ValidatedObject.Code = orderDto.Code;

            return validator;
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

        static private async Task createOrderItemAsync(OrderDto orderDto, Validator<OrderDto, Order> orderValidator)
        {
            List<string> errors = null;
            if (orderDto.ProductItems == null)
            {
                return;
            }

            foreach (var item in orderDto.ProductItems)
            {
                var validatorSingleItem = await OrderItem.CreateOrderItemAsync(item);

                if (!validatorSingleItem.IsValid)
                {
                    errors = validatorSingleItem.BrokenRules.SelectMany(i => i.Errors.Select(k => k.Code)).ToList();
                }
                else
                {
                    orderValidator.ValidatedObject._orderItems.Add(validatorSingleItem.ValidatedObject);
                }  
            }
            if (errors != null &&
                errors.Count > 0)
            {
                orderValidator.AddCustomBrokenRule(errors.Select(i => new ValidatorError
                {
                    Code = "InvalidProductItem",
                    Detail = new ValidatorErrorDetail { Messages = new List<string> { i } },
                    GeneratorClass = "OrderFactory",
                    Type = ValidatorType.DomainEntity
                }).ToList()
                );
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
