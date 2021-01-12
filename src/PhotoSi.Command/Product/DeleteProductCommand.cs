using DomainModel.Interfaces;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Product.CommandResult;
using PhotoSi.Interfaces.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Command.Product
{
    public class DeleteProductCommand : ICommand<DeleteProductResult>
    {
        public DeleteProductCommand()
        {
        }

        public int ProductId { get; set; }

        public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResult>
        {
            private readonly ILogger<DeleteProductHandler> _logger;
            private readonly IProductRepository _productRepository;

            public DeleteProductHandler(ILogger<DeleteProductHandler> logger,
                                        IProductRepository productRepository)
            {
                _logger = logger;
                _productRepository = productRepository;
            }

            public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var productToRemove = await _productRepository.GetByIdAsync(request.ProductId);
                if (productToRemove == null)
                {
                    return new DeleteProductResult
                    {
                        Errors = new List<string> { $"Product id {request.ProductId} not found" },
                        HaveError = true
                    };
                }

                _logger.LogDebug("remove to repository");
                _productRepository.Delete(productToRemove);

                _logger.LogDebug("SaveChangeAsync");
                await _productRepository.SaveChangeAsync();

                return new DeleteProductResult
                {
                    ProductId = request.ProductId,
                    HaveError = false
                };
            }

        }
    }
}
