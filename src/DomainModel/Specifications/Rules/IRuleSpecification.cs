using DomainModel.Validators;
using System.Threading.Tasks;

namespace DomainModel.Specifications.Rules
{
    public interface IRuleSpecification<T>
    {
        Task<ValidatorResult> IsSatisfiedAsync(T subject);
    }
}
