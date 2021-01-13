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
    public class OrderAcceptOnlyOneProductForCategorySpecification : IRuleSpecification<OrderDto>
    {
        //Potrei usare i generic per creare un'unica classe per tutte le entità
        //Però le logiche potrebbero anche essere completamente diverse
        //Quindi lascio uno specifico per ogni entità

        private readonly IRepository<Product> _product;

        public OrderAcceptOnlyOneProductForCategorySpecification(IRepository<Product> product)
        {
            _product = product;
        }

        public async Task<ValidatorResult> IsSatisfiedAsync(OrderDto orderDto)
        {
            var result = new ValidatorResult { IsSatisfied = true };
            if (_product == null)
            {
                return result;
            }

            var allProducts = await _product.FindAsync(new ProductFilterByIdsSpecification(orderDto?.ProductItems?.Select(i => i.ProductId).ToList()));
            if (allProducts == null)
            {
                return result;
            }

            var categories = allProducts.GroupBy(i => i.CategoryId).Select(group => new
            {
                CategoryId = group.Key,
                Count = group.Count()
            }).Where(i => i.Count > 1);

            if (categories != null &&
                categories.Any())
            {
                result.IsSatisfied = false;
                result.Errors = categories.Select(i =>
                       new ValidatorError
                       {
                           Code = $"{ErrorCode.TooManyProducrForCategory}_{i.CategoryId}",
                           Detail = new ValidatorErrorDetail { JsonData = "", Messages = new List<string> { $"Too Many Product For Same Category {i.CategoryId}" } },
                           Type = ValidatorType.Business,
                           GeneratorClass = GetType().FullName
                       }).ToList();
            }

            return result;
        }
    }
}
