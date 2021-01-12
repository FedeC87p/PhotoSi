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
    public class OptionUniqueNameSpecification : IRuleSpecification<OptionDto>
    {
        //Potrei usare i generic per creare un'unica classe per tutte le entità
        //Però le logiche potrebbero anche essere completamente diverse
        //Quindi lascio uno specifico per ogni entità

        private readonly IRepository<Option> _option;

        public OptionUniqueNameSpecification(IRepository<Option> option)
        {
            _option = option;
        }

        public async Task<ValidatorResult> IsSatisfiedAsync(OptionDto option)
        {
            var result = new ValidatorResult { IsSatisfied = true };
            if (_option == null)
            {
                return result;
            }

            var allOptions = await _option.FindAsync(new OptionByNameSpecification(option.Name, option.OptionId));
            if (allOptions == null ||
                !allOptions.Any())
            {
                return result;
            }

            result.IsSatisfied = false;
            result.Errors = new List<ValidatorError> {
                    new ValidatorError {
                        Code = ErrorCode.DuplicateName,
                        Detail = new ValidatorErrorDetail { JsonData = "", Messages = new List<string> { $"Option duplicate: {option.Name}" }  },
                        Type = ValidatorType.Rules,
                        GeneratorClass = GetType().FullName
                    }
                };

            return result;
        }
    }
}
