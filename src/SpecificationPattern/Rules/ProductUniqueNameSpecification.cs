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
    public class ProductUniqueNameSpecification : IRuleSpecification<ProductDto>
    {
        //Potrei usare i generic per creare un'unica classe per tutte le entità
        //Però le logiche potrebbero anche essere completamente diverse
        //Quindi lascio uno specifico per ogni entità

        private readonly IRepository<Product> _product;

        public ProductUniqueNameSpecification(IRepository<Product> product)
        {
            _product = product;
        }

        public async Task<ValidatorResult> IsSatisfiedAsync(ProductDto product)
        {
            var result = new ValidatorResult { IsSatisfied = true };
            if (_product == null)
            {
                return result;
            }

            var allProducts = await _product.FindAsync(new ProductByNameSpecification(product.Name, product.ProductId));
            if (allProducts == null ||
                !allProducts.Any())
            {
                return result;
            }

            result.IsSatisfied = false;
            result.Errors = new List<ValidatorError> {
                    new ValidatorError {
                        Code = ErrorCode.DuplicateName,
                        Detail = new ValidatorErrorDetail { JsonData = "", Messages = new List<string> { $"Name duplicate: {product.Name}" }  },
                        Type = ValidatorType.Rules,
                        GeneratorClass = GetType().FullName
                    }
                };

            return result;
        }
    }
}
