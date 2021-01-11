using DomainModel.Dtos;
using DomainModel.Interfaces;
using DomainModel.Specifications.Rules;
using DomainModel.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities.Products
{
    public class Category : BaseEntity, IAggregateRoot
    {
        public int CategoryId { get; protected set; }
        public string Name { get; private set; }
        public string Descrizione { get; private set; }

        public static async Task<IValidator<CategoryDto, Category>> CreateCategoryAsync(CategoryDto categoryDto, IEnumerable<IRuleSpecification<CategoryDto>> rules)
        {
            var validator = new Validator<CategoryDto, Category>(rules);
            await validator.ExecuteCheckAsync(categoryDto, new Category());

            if (!validator.IsValid)
            {
                return validator;
            }
            validator.ValidateObject.SetName(categoryDto.Name);

            return validator;
        }

        private void SetName(string name)
        {
            //Il name si può settare solo privatamente in modo da validarlo con regole dinamiche 
            Name = name;
        }

        public bool SetDescrizione(string descrizione)
        {
            //Esempio di regola non configurabile ma cablata nel codice
            if (descrizione != null &&
                descrizione.Length > 250)
            {
                return false;
            }

            return true;
        }

    }
}
