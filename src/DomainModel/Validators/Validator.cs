using DomainModel.Specifications.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Validators
{
    public class Validator<T, TResult> : IValidator<T, TResult>
    {
        private readonly IEnumerable<IRuleSpecification<T>> _rules;
        private List<ValidatorResult> _ruleResults;
        private bool _executeRun;
        private TResult _validateObject;

        public Validator(IEnumerable<IRuleSpecification<T>> rules)
        {
            _rules = rules;
        }

        public async Task ExecuteCheckAsync(T dtoEntity, TResult viewTemplate)
        {
            _validateObject = viewTemplate;
            _executeRun = true;
            _ruleResults = new List<ValidatorResult>();
            if (_rules == null)
            {
                return;
            }

            foreach (var item in _rules)
            {
                var isSatisfied = await item.IsSatisfiedAsync(dtoEntity);
                if (!isSatisfied.IsSatisfied)
                {
                    _ruleResults.Add(isSatisfied);
                }
            }
        }

        public bool IsValid
        {
            get
            {
                if (!_executeRun)
                {
                    throw new Exception("ExecuteCheckAsync() before to call IsValid()");
                }

                if (_ruleResults == null ||
                    !_ruleResults.Any())
                {
                    return true;
                }

                return false;
            }
        }

        public List<ValidatorResult> BrokenRules
        {
            get
            {
                if (!_executeRun)
                {
                    throw new Exception("ExecuteCheckAsync() before to call IsValid()");
                }

                return _ruleResults;
            }
        }

        public void AddCustomBrokenRule(List<ValidatorError> validatorErrors)
        {
            if (!_executeRun)
            {
                throw new Exception("ExecuteCheckAsync() before to call AddCustomBrokenRule()");
            }
            AddCustomBrokenRule(new ValidatorResult { IsSatisfied = false, Errors = validatorErrors });
        }

        public void AddCustomBrokenRule(ValidatorResult validatorResult)
        {
            if (!_executeRun)
            {
                throw new Exception("ExecuteCheckAsync() before to call AddCustomBrokenRule()");
            }

            _ruleResults.Add(validatorResult);
        }

        public TResult ValidatedObject => IsValid ? _validateObject : default(TResult);
    }
}
