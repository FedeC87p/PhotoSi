using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Categories.CommandResult;
using PhotoSi.Interfaces.Mediator;
using SpecificationPattern;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Command.Categories
{
    public class DeleteCategoryCommand : ICommand<DeleteCategoryResult>
    {
        public DeleteCategoryCommand()
        {
        }

        public int CategoryId { get; set; }

        public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, DeleteCategoryResult>
        {
            private readonly ILogger<DeleteCategoryHandler> _logger;
            private readonly IRepository<Category> _categoryRepository;
            private readonly IRepository<DomainModel.Entities.Products.Product> _productRepository;

            public DeleteCategoryHandler(ILogger<DeleteCategoryHandler> logger,
                                        IRepository<Category> categoryRepository,
                                        IRepository<DomainModel.Entities.Products.Product> productRepository)
            {
                _logger = logger;
                _categoryRepository = categoryRepository;
                _productRepository = productRepository;
            }

            public async Task<DeleteCategoryResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var productToRemove = await _categoryRepository.GetByIdAsync(request.CategoryId);
                if (productToRemove == null)
                {
                    return new DeleteCategoryResult
                    {
                        Errors = new List<string> { $"Category id {request.CategoryId} not found" },
                        HaveError = true
                    };
                }

                var products = await _productRepository.FindAsync(new ProductByCategoryIdSpecification(request.CategoryId));
                if (products.Count > 0)
                {
                    return new DeleteCategoryResult
                    {
                        Errors = new List<string> { $"Category id {request.CategoryId} contains product" },
                        HaveError = true
                    };
                }

                _logger.LogDebug("remove to repository");
                _categoryRepository.Delete(productToRemove);

                _logger.LogDebug("SaveChangeAsync");
                await _categoryRepository.SaveChangeAsync();

                return new DeleteCategoryResult
                {
                    CategoryId = request.CategoryId,
                    HaveError = false
                };
            }

        }
    }
}
