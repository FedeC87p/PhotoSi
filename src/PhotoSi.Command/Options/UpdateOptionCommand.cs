using DomainModel.Dtos;
using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using DomainModel.Specifications.Rules;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Options.CommandResult;
using PhotoSi.Interfaces.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Command.Options
{
    public class UpdateOptionCommand : ICommand<UpdateOptionResult>
    {
        public UpdateOptionCommand()
        {
        }

        public OptionDto Option { get; set; }

        public class UpdateOptionHandler : IRequestHandler<UpdateOptionCommand, UpdateOptionResult>
        {
            private readonly ILogger<UpdateOptionHandler> _logger;
            private readonly IEnumerable<IRuleSpecification<OptionDto>> _rules;
            private readonly IRepository<Option> _optionRepository;

            public UpdateOptionHandler(ILogger<UpdateOptionHandler> logger,
                                        IRepository<Option> optionRepository,
                                        IEnumerable<IRuleSpecification<OptionDto>> rules)
            {
                _logger = logger;
                _optionRepository = optionRepository;
                _rules = rules;
            }

            public async Task<UpdateOptionResult> Handle(UpdateOptionCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var optionToEdit = await _optionRepository.GetByIdAsync(request.Option.OptionId);
                if (optionToEdit == null)
                {
                    return new UpdateOptionResult
                    {
                        Errors = new List<string> { $"Option id {request.Option.OptionId} not found" },
                        HaveError = true
                    };
                }

                var validator = await optionToEdit.EditAsync(request.Option, _rules);

                if (validator?.ValidatedObject == null || 
                    !validator.IsValid)
                {
                    return new UpdateOptionResult
                    {
                        Errors = validator.BrokenRules.SelectMany(i => i.Errors.Select(k => k.Code)).ToList(),
                        HaveError = true
                    };
                }
                optionToEdit = validator.ValidatedObject;


                _logger.LogDebug("edit to repository");
                _optionRepository.Update(optionToEdit);
                _logger.LogDebug("SaveChangeAsync");
                await _optionRepository.SaveChangeAsync();

                return new UpdateOptionResult
                {
                    OptionId = validator.ValidatedObject.OptionId,
                    HaveError = false
                };
            }

        }
    }
}
