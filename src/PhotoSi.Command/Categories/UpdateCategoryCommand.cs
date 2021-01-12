using DomainModel.Dtos;
using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using DomainModel.Specifications.Rules;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Categories.CommandResult;
using PhotoSi.Command.Options.CommandResult;
using PhotoSi.Command.Product.CommandResult;
using PhotoSi.Interfaces.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Command.Categories
{
    public class UpdateCategoryCommand : ICommand<UpdateCategoryResult>
    {
        public UpdateCategoryCommand()
        {
        }

        public CategoryDto Category { get; set; }

        public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResult>
        {
            private readonly ILogger<UpdateCategoryHandler> _logger;
            private readonly IEnumerable<IRuleSpecification<CategoryDto>> _rules;
            private readonly IRepository<Category> _categoryRepository;

            public UpdateCategoryHandler(ILogger<UpdateCategoryHandler> logger,
                                        IRepository<Category> categoryRepository,
                                        IEnumerable<IRuleSpecification<CategoryDto>> rules)
            {
                _logger = logger;
                _categoryRepository = categoryRepository;
                _rules = rules;
            }

            public async Task<UpdateCategoryResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var categoryToEdit = await _categoryRepository.GetByIdAsync(request.Category.CategoryId);
                if (categoryToEdit == null)
                {
                    return new UpdateCategoryResult
                    {
                        Errors = new List<string> { $"Category id {request.Category.CategoryId} not found" },
                        HaveError = true
                    };
                }

                var validator = await categoryToEdit.EditAsync(request.Category, _rules);

                if (validator?.ValidatedObject == null || 
                    !validator.IsValid)
                {
                    return new UpdateCategoryResult
                    {
                        CategoryId = request.Category.CategoryId,
                        Errors = validator.BrokenRules.SelectMany(i => i.Errors.Select(k => k.Code)).ToList(),
                        HaveError = true
                    };
                }
                categoryToEdit = validator.ValidatedObject;


                _logger.LogDebug("edit to repository");
                _categoryRepository.Update(categoryToEdit);
                _logger.LogDebug("SaveChangeAsync");
                await _categoryRepository.SaveChangeAsync();

                return new UpdateCategoryResult
                {
                    CategoryId = validator.ValidatedObject.CategoryId,
                    HaveError = false
                };
            }

        }
    }
}
