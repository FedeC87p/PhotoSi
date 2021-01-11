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
    public class Option : BaseEntity, IAggregateRoot
    {
        public int OptionId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Note { get;  set; } //esempio di dato pubblico senza metodo di set

        private readonly List<Product> _products = new List<Product>();
        public virtual IReadOnlyCollection<Product> Products => _products?.AsReadOnly();

        public static async Task<IValidator<OptionDto, Option>> CreateOptionAsync(OptionDto optionDto, IEnumerable<IRuleSpecification<OptionDto>> rules)
        {
            var validator = new Validator<OptionDto, Option>(rules);
            await validator.ExecuteCheckAsync(optionDto, new Option());

            //Qui potrebbero essere inserite delle rules a livello di codice da richiamare sempre  
            //(oppure prima del controllo delle rules dinamiche)

            if (!validator.IsValid)
            {
                return validator;
            }
            validator.ValidatedObject.SetName(optionDto.Name);
            validator.ValidatedObject.SetDescription(optionDto.Name);

            return validator;
        }

        private void SetName(string name)
        {
            //Il name si può settare solo privatamente in modo da validarlo con regole dinamiche 
            Name = name;
        }

        public void SetDescription(string description)
        {
            //Esempio di dato che non ha nessuna validatione
            Description = description;
        }

    }
}
