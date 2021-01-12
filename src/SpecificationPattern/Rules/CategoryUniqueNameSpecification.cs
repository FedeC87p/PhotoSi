using DomainModel.Dtos;
using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using DomainModel.Specifications.Rules;
using DomainModel.Validators;
using SpecificationPattern.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationPattern.Rules
{
    public class CategoryUniqueNameSpecification : IRuleSpecification<CategoryDto>
    {
        //Potrei usare i generic per creare un'unica classe per tutte le entità
        //Però le logiche potrebbero anche essere completamente diverse
        //Quindi lascio uno specifico per ogni entità

        private readonly IRepository<Category> _category;

        public CategoryUniqueNameSpecification(IRepository<Category> category)
        {
            _category = category;
        }

        public async Task<ValidatorResult> IsSatisfiedAsync(CategoryDto category)
        {
            var result = new ValidatorResult { IsSatisfied = true };
            if (_category == null)
            {
                return result;
            }

            var allCategories = await _category.FindAsync(new CategoryByNameSpecification(category.Name, category.CategoryId));
            if (allCategories == null ||
                !allCategories.Any())
            {
                return result;
            }

            result.IsSatisfied = false;
            result.Errors = new List<ValidatorError> {
                    new ValidatorError {
                        Code = ErrorCode.DuplicateName,
                        Detail = new ValidatorErrorDetail { JsonData = "", Messages = new List<string> { $"Category duplicate: {category.Name}" }  },
                        Type = ValidatorType.Rules,
                        GeneratorClass = GetType().FullName
                    }
                };

            return result;
        }
    }
}
