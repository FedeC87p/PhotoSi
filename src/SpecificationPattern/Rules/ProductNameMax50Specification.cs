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
    public class ProductNameMax50Specification : IRuleSpecification<ProductDto>
    {
        //Dovrei usare i generic per creare un'unica classe per tutte le entità

        public ProductNameMax50Specification()
        {
        }

        public async Task<ValidatorResult> IsSatisfiedAsync(ProductDto product)
        {
            var result = new ValidatorResult { IsSatisfied = true };
            if (!string.IsNullOrWhiteSpace(product.Name) &&
                product.Name.Trim().Length <= 50)
            {
                return result;
            }

            result.IsSatisfied = false;
            result.Errors = new List<ValidatorError> {
                    new ValidatorError {
                        Code = ErrorCode.LenghtName50,
                        Detail = new ValidatorErrorDetail { JsonData = "", Messages = new List<string> { $"Name lenght min 1 max  50, actual is: {product?.Name?.Length ?? 0}" }  },
                        Type = ValidatorType.Rules,
                        GeneratorClass = GetType().FullName
                    }
                };

            return result;
        }
    }
}
