using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Validators
{
    public class ValidatorResult
    {
        public bool IsSatisfied { get; set; }
        public List<ValidatorError> Errors { get; set; }
    }
    
}
