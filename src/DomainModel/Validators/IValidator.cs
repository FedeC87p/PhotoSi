using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainModel.Validators
{
    public interface IValidator<T, TResult>
    {
        Task ExecuteCheckAsync(T dtoEntity, TResult validateObject);
        bool IsValid { get; }
        List<ValidatorResult> BrokenRules { get; }
        TResult ValidateObject { get; }
    }
}
