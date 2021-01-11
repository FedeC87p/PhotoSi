using DomainModel.Dtos;
using DomainModel.Entities.Products;
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
            private readonly IProductRepository _productRepository;
            private readonly IRepository<Option> _optionRepository;

            public CreateProductHandler(ILogger<CreateProductHandler> logger,
                                        IProductRepository productRepository,
                                        IRepository<Option> optionRepository,
                                        IEnumerable<IRuleSpecification<ProductDto>> rules)
            {
                _logger = logger;
                _productRepository = productRepository;
                _optionRepository = optionRepository;
                _rules = rules;
            }

            public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var validator = await DomainModel.Entities.Products.Product.CreateProductAsync(request.Product, _rules);

                if (validator?.ValidatedObject == null || 
                    !validator.IsValid)
                {
                    return new CreateProductResult
                    {
                        Errors = validator.BrokenRules.SelectMany(i => i.Errors.Select(k => k.Code)).ToList(),
                        HaveError = true
                    };
                }

                var optionsEntity = new List<Option>();
                var optionsErrors = new List<string>();
                if (request.Product.OptionsId != null)
                {
                    foreach (var id in request.Product.OptionsId)
                    {
                        var optionEntity = await _optionRepository.GetByIdAsync(id);
                        if (optionEntity != null)
                        {
                            optionsEntity.Add(optionEntity);
                        }
                        else
                        {
                            optionsErrors.Add($"Option with id {id} not fpund");
                        }
                    }
                }

                if (optionsErrors.Any())
                {
                    return new CreateProductResult
                    {
                        Errors = optionsErrors,
                        HaveError = true
                    };
                }


                _logger.LogDebug("add to repository");
                _productRepository.Add(validator.ValidatedObject);

                foreach (var option in optionsEntity)
                {
                    _productRepository.LinkOption(validator.ValidatedObject, option);
                }

                _logger.LogDebug("SaveChangeAsync");
                await _productRepository.SaveChangeAsync();

                return new CreateProductResult
                {
                    ProductId = validator.ValidatedObject.ProductId,
                    HaveError = false
                };
            }

        }
    }
}
