using DomainModel.Dtos;
using DomainModel.Interfaces;
using DomainModel.Specifications.Rules;
using DomainModel.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Entities.Products
{
    public class Product : BaseEntity, IAggregateRoot
    {
        public int ProductId { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Note { get; set; } //esempio di dato pubblico senza metodo di set
        public int CategoryId { get; protected set; }


        public virtual Category Category { get; protected set; }

        private readonly List<Option> _options = new List<Option>();
        public virtual IReadOnlyCollection<Option> Options => _options?.AsReadOnly();

        public static async Task<IValidator<ProductDto, Product>> CreateProductAsync(ProductDto productDto, IEnumerable<IRuleSpecification<ProductDto>> rules)
        {
            var validator = new Validator<ProductDto, Product>(rules);
            await validator.ExecuteCheckAsync(productDto, new Product());

            if (!validator.IsValid)
            {
                return validator;
            }
            validator.ValidatedObject.SetName(productDto.Name);
            validator.ValidatedObject.SetDescription(productDto.Description);
            validator.ValidatedObject.Note = productDto.Note;

            validator.ValidatedObject.SetCategory(productDto.CategoryId);

            return validator;
        }

        public async Task<IValidator<ProductDto, Product>> EditAsync(ProductDto productDto, IEnumerable<IRuleSpecification<ProductDto>> rules)
        {
            var validator = new Validator<ProductDto, Product>(rules);
            await validator.ExecuteCheckAsync(productDto, this);

            //Qui potrebbero essere inserite delle rules a livello di codice da richiamare sempre  
            //(oppure prima del controllo delle rules dinamiche)

            if (!validator.IsValid)
            {
                return validator;
            }

            validator.ValidatedObject.SetName(productDto.Name);
            validator.ValidatedObject.SetDescription(productDto.Name);
            validator.ValidatedObject.Note = productDto.Note;


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

        public void SetCategory(Category category)
        {
            if (category == null)
            {
                return;
            }
            CategoryId = category.CategoryId;
        }

        public void SetCategory(int categoryId)
        {
            CategoryId = categoryId;
        }

        public void AssignOption(Option option)
        {
            if (option == null)
            {
                return;
            }

            var exist = _options?.Any(i => i.OptionId == option.OptionId);
            if (exist.HasValue && exist.Value)
            {
                return;
            }

            _options.Add(option);
        }


        public void UnAssignOption(Option option)
        {
            _options.RemoveAll(i => i.OptionId == option.OptionId);
        }

        public void UnAssignAllOptions()
        {
            _options.Clear();
        }
    }
}
