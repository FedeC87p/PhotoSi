using DomainModel.Dtos;
using DomainModel.Entities.Orders;
using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using DomainModel.Specifications.Rules;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Orders.CommandResult;
using PhotoSi.Command.Product.CommandResult;
using PhotoSi.Interfaces.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Command.Orders
{
    public class CreateOrderCommand : ICommand<CreateOrderResult>
    {
        public CreateOrderCommand()
        {
        }

        public OrderDto Order { get; set; }

        public class CreateProductHandler : IRequestHandler<CreateOrderCommand, CreateOrderResult>
        {
            private readonly ILogger<CreateProductHandler> _logger;
            private readonly IEnumerable<IRuleSpecification<OrderDto>> _rulesOrder;
            private readonly IEnumerable<IRuleSpecification<OrderItemDto>> _rulesOrderItem;
            private readonly IEnumerable<IRuleSpecification<OrderItemOptionDto>> _rulesOrderOptionItem;
            private readonly IProductRepository _productRepository;
            private readonly IRepository<Order> _orderRepository;

            public CreateProductHandler(ILogger<CreateProductHandler> logger,
                                        IProductRepository productRepository,
                                        IRepository<Order> orderRepository,
                                        IEnumerable<IRuleSpecification<OrderDto>> rulesOrder,
                                        IEnumerable<IRuleSpecification<OrderItemDto>> rulesOrderItem,
                                        IEnumerable<IRuleSpecification<OrderItemOptionDto>> rulesOrderOptionItem)
            {
                _logger = logger;
                _productRepository = productRepository;
                _orderRepository = orderRepository;
                _rulesOrder = rulesOrder;
                _rulesOrderItem = rulesOrderItem;
                _rulesOrderOptionItem = rulesOrderOptionItem;
            }

            public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var validator = await DomainModel.Entities.Orders.Order.CreateOrderAsync(request.Order, _rulesOrder, _rulesOrderItem, _rulesOrderOptionItem);

                //TODO qui controlliamo la correttezza dei dati inviati dal client

                if (validator?.ValidatedObject == null || 
                    !validator.IsValid)
                {
                    return new CreateOrderResult
                    {
                        Errors = validator.BrokenRules.SelectMany(i => i.Errors.Select(k => k.Code)).ToList(),
                        HaveError = true
                    };
                }

                _logger.LogDebug("add to repository");
                _orderRepository.Add(validator.ValidatedObject);

                _logger.LogDebug("SaveChangeAsync");
                await _productRepository.SaveChangeAsync();

                return new CreateOrderResult
                {
                    OrderId = validator.ValidatedObject.OrderId,
                    HaveError = false
                };
            }

        }
    }
}
