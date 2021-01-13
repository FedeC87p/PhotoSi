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
    public class OrderItemAssociatedOnlyWithLinkedOptionItemSpecification : IRuleSpecification<OrderItemDto>
    {
        //Potrei usare i generic per creare un'unica classe per tutte le entità
        //Però le logiche potrebbero anche essere completamente diverse
        //Quindi lascio uno specifico per ogni entità

        private readonly IRepository<Product> _product;

        public OrderItemAssociatedOnlyWithLinkedOptionItemSpecification(IRepository<Product> product)
        {
            _product = product;
        }

        public async Task<ValidatorResult> IsSatisfiedAsync(OrderItemDto orderItemDto)
        {
            var result = new ValidatorResult { IsSatisfied = true };
            if (_product == null)
            {
                return result;
            }

            var allProducts = await _product.FindAsync(new ProductFilterByIdsSpecification(new List<int> { orderItemDto.ProductId }, includeOptions: true));
            var product = allProducts?.FirstOrDefault();
            if (product == null)
            {
                return result;
            }

            var containedValidLinkedOptionItem = !orderItemDto.OptionItems.Select(i => i.OptionId).Except(product.Options.Select(i => i.OptionId)).Any();

            if (!containedValidLinkedOptionItem)
            {
                result.IsSatisfied = false;
                result.Errors = new List<ValidatorError> {
                    new ValidatorError
                    {
                        Code = $"{ErrorCode.InvalidLinkedOrderItemToOptionItem}_{product.ProductId}",
                        Detail = new ValidatorErrorDetail { JsonData = "", Messages = new List<string> { $"Product id {product.ProductId} contains invalid option" } },
                        Type = ValidatorType.Business,
                        GeneratorClass = GetType().FullName
                    }
                };
            }

            return result;
        }
    }
}
