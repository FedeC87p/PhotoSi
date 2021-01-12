using DomainModel.Dtos;
using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using DomainModel.Specifications.Rules;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Options.CommandResult;
using PhotoSi.Command.Product.CommandResult;
using PhotoSi.Interfaces.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Command.Options
{
    public class CreateOptionCommand : ICommand<CreateOptionResult>
    {
        public CreateOptionCommand()
        {
        }

        public OptionDto Option { get; set; }

        public class CreateOptionHandler : IRequestHandler<CreateOptionCommand, CreateOptionResult>
        {
            private readonly ILogger<CreateOptionHandler> _logger;
            private readonly IEnumerable<IRuleSpecification<OptionDto>> _rules;
            private readonly IRepository<Option> _optionRepository;

            public CreateOptionHandler(ILogger<CreateOptionHandler> logger,
                                        IRepository<Option> optionRepository,
                                        IEnumerable<IRuleSpecification<OptionDto>> rules)
            {
                _logger = logger;
                _optionRepository = optionRepository;
                _rules = rules;
            }

            public async Task<CreateOptionResult> Handle(CreateOptionCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var validator = await DomainModel.Entities.Products.Option.CreateOptionAsync(request.Option, _rules);

                if (validator?.ValidatedObject == null || 
                    !validator.IsValid)
                {
                    return new CreateOptionResult
                    {
                        Errors = validator.BrokenRules.SelectMany(i => i.Errors.Select(k => k.Code)).ToList(),
                        HaveError = true
                    };
                }

                _logger.LogDebug("add to repository");
                _optionRepository.Add(validator.ValidatedObject);

                _logger.LogDebug("SaveChangeAsync");
                await _optionRepository.SaveChangeAsync();

                return new CreateOptionResult
                {
                    OptionId = validator.ValidatedObject.OptionId,
                    HaveError = false
                };
            }

        }
    }
}
