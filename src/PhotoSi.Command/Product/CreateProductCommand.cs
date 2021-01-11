using DomainModel.Dtos;
using DomainModel.Interfaces;
using DomainModel.Specifications.Rules;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Product.CommandResult;
using PhotoSi.Interfaces.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Command.Product
{
    public class CreateProductCommand : ICommand<CreateProductResult>
    {
        public CreateProductCommand()
        {
        }

        public ProductDto Product { get; set; }

        public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
        {
            private readonly ILogger<CreateProductHandler> _logger;
            private readonly IEnumerable<IRuleSpecification<ProductDto>> _rules;
            private readonly IMediator _mediator;
            private readonly IRepository<DomainModel.Entities.Products.Product> _repository;

            public CreateProductHandler(ILogger<CreateProductHandler> logger,
                                        IMediator mediator,
                                        IRepository<DomainModel.Entities.Products.Product> repository, 
                                        IEnumerable<IRuleSpecification<ProductDto>> rules)
            {
                _logger = logger;
                _mediator = mediator;
                _repository = repository;
                _rules = rules;
            }

            public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var validator = await DomainModel.Entities.Products.Product.CreateCategoryAsync(request.Product, _rules);

                if (validator?.ValidateObject == null || 
                    !validator.IsValid)
                {
                    return new CreateProductResult
                    {
                        Errors = validator.BrokenRules.SelectMany(i => i.Errors.Select(k => k.Code)).ToList(),
                        HaveError = true
                    };
                }

                _logger.LogDebug("add to repository");
                _repository.Add(validator.ValidateObject);

                _logger.LogDebug("SaveChangeAsync");
                await _repository.SaveChangeAsync();

                return new CreateProductResult
                {
                    ProductId = validator.ValidateObject.ProductId,
                    HaveError = false
                };
            }

        }
    }
}
