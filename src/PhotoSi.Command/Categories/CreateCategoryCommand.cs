using DomainModel.Dtos;
using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using DomainModel.Specifications.Rules;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Categories.CommandResult;
using PhotoSi.Interfaces.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Command.Categories
{
    public class CreateCategoryCommand : ICommand<CreateCategoryResult>
    {
        public CreateCategoryCommand()
        {
        }

        public CategoryDto Category { get; set; }

        public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResult>
        {
            private readonly ILogger<CreateCategoryHandler> _logger;
            private readonly IEnumerable<IRuleSpecification<CategoryDto>> _rules;
            private readonly IRepository<Category> _categoryRepository;

            public CreateCategoryHandler(ILogger<CreateCategoryHandler> logger,
                                        IRepository<Category> categoryRepository,
                                        IEnumerable<IRuleSpecification<CategoryDto>> rules)
            {
                _logger = logger;
                _categoryRepository = categoryRepository;
                _rules = rules;
            }

            public async Task<CreateCategoryResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var validator = await DomainModel.Entities.Products.Category.CreateCategoryAsync(request.Category, _rules);

                if (validator?.ValidatedObject == null || 
                    !validator.IsValid)
                {
                    return new CreateCategoryResult
                    {
                        Errors = validator.BrokenRules.SelectMany(i => i.Errors.Select(k => k.Code)).ToList(),
                        HaveError = true
                    };
                }

                _logger.LogDebug("add to repository");
                _categoryRepository.Add(validator.ValidatedObject);

                _logger.LogDebug("SaveChangeAsync");
                await _categoryRepository.SaveChangeAsync();

                return new CreateCategoryResult
                {
                    CategoryId = validator.ValidatedObject.CategoryId,
                    HaveError = false
                };
            }

        }
    }
}
