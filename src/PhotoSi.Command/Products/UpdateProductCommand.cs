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
    public class UpdateProductCommand : ICommand<UpdateProductResult>
    {
        public UpdateProductCommand()
        {
        }

        public ProductDto Product { get; set; }

        public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
        {
            private readonly ILogger<UpdateProductHandler> _logger;
            private readonly IEnumerable<IRuleSpecification<ProductDto>> _rules;
            private readonly IProductRepository _productRepository;
            private readonly IRepository<Option> _optionRepository;

            public UpdateProductHandler(ILogger<UpdateProductHandler> logger,
                                        IProductRepository productRepository,
                                        IRepository<Option> optionRepository,
                                        IEnumerable<IRuleSpecification<ProductDto>> rules)
            {
                _logger = logger;
                _productRepository = productRepository;
                _optionRepository = optionRepository;
                _rules = rules;
            }

            public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var productToEdit = await _productRepository.GetByIdAsync(request.Product.ProductId);
                if (productToEdit == null)
                {
                    return new UpdateProductResult
                    {
                        Errors = new List<string> { $"Product id {request.Product.ProductId} not found" },
                        HaveError = true
                    };
                }

                var validator = await productToEdit.EditAsync(request.Product, _rules);

                if (validator?.ValidatedObject == null || 
                    !validator.IsValid)
                {
                    return new UpdateProductResult
                    {
                        Errors = validator.BrokenRules.SelectMany(i => i.Errors.Select(k => k.Code)).ToList(),
                        HaveError = true
                    };
                }
                productToEdit = validator.ValidatedObject;

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
                            optionsErrors.Add($"Option with id {id} not found");
                        }
                    }
                }

                if (optionsErrors.Any())
                {
                    return new UpdateProductResult
                    {
                        Errors = optionsErrors,
                        HaveError = true
                    };
                }


                _logger.LogDebug("edit to repository");
                productToEdit.UnAssignAllOptions(); //Sarebbe meglio aggiungere quelle non già presenti (e rimuovere quelle no più presenti)
                _productRepository.Update(productToEdit);

                
                foreach (var option in optionsEntity)
                {
                    _productRepository.LinkOption(validator.ValidatedObject, option);
                }

                _logger.LogDebug("SaveChangeAsync");
                await _productRepository.SaveChangeAsync();

                return new UpdateProductResult
                {
                    ProductId = validator.ValidatedObject.ProductId,
                    HaveError = false
                };
            }

        }
    }
}
