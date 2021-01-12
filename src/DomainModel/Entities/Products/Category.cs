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
        public string Description { get; private set; }
        public string Note { get; set; } //esempio di dato pubblico senza metodo di set

        public static async Task<IValidator<CategoryDto, Category>> CreateCategoryAsync(CategoryDto categoryDto, IEnumerable<IRuleSpecification<CategoryDto>> rules)
        {
            var validator = new Validator<CategoryDto, Category>(rules);
            await validator.ExecuteCheckAsync(categoryDto, new Category());

            //Qui potrebbero essere inserite delle rules a livello di codice da richiamare sempre  
            //(oppure prima del controllo delle rules dinamiche)

            if (!validator.IsValid)
            {
                return validator;
            }

            validator.ValidatedObject.SetName(categoryDto.Name);
            validator.ValidatedObject.SetDescription(categoryDto.Description);
            validator.ValidatedObject.Note = categoryDto.Note;

            return validator;
        }

        public async Task<IValidator<CategoryDto, Category>> EditAsync(CategoryDto categoryDto, IEnumerable<IRuleSpecification<CategoryDto>> rules)
        {
            var validator = new Validator<CategoryDto, Category>(rules);
            await validator.ExecuteCheckAsync(categoryDto, this);

            //Qui potrebbero essere inserite delle rules a livello di codice da richiamare sempre  
            //(oppure prima del controllo delle rules dinamiche)

            if (!validator.IsValid)
            {
                return validator;
            }

            validator.ValidatedObject.SetName(categoryDto.Name);
            validator.ValidatedObject.SetDescription(categoryDto.Description);
            validator.ValidatedObject.Note = categoryDto.Note;


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
