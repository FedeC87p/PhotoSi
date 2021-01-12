using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Categories.CommandResult;
using PhotoSi.Interfaces.Mediator;
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

            public DeleteCategoryHandler(ILogger<DeleteCategoryHandler> logger,
                                        IRepository<Category> categoryRepository)
            {
                _logger = logger;
                _categoryRepository = categoryRepository;
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
