using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Options.CommandResult;
using PhotoSi.Interfaces.Mediator;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Command.Options
{
    public class DeleteOptionCommand : ICommand<DeleteOptionResult>
    {
        public DeleteOptionCommand()
        {
        }

        public int OptionId { get; set; }

        public class DeleteOptionHandler : IRequestHandler<DeleteOptionCommand, DeleteOptionResult>
        {
            private readonly ILogger<DeleteOptionHandler> _logger;
            private readonly IRepository<Option> _optionRepository;

            public DeleteOptionHandler(ILogger<DeleteOptionHandler> logger,
                                        IRepository<Option> optionRepository)
            {
                _logger = logger;
                _optionRepository = optionRepository;
            }

            public async Task<DeleteOptionResult> Handle(DeleteOptionCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var productToRemove = await _optionRepository.GetByIdAsync(request.OptionId);
                if (productToRemove == null)
                {
                    return new DeleteOptionResult
                    {
                        Errors = new List<string> { $"Option id {request.OptionId} not found" },
                        HaveError = true
                    };
                }

                _logger.LogDebug("remove to repository");
                _optionRepository.Delete(productToRemove);

                _logger.LogDebug("SaveChangeAsync");
                await _optionRepository.SaveChangeAsync();

                return new DeleteOptionResult
                {
                    OptionId = request.OptionId,
                    HaveError = false
                };
            }

        }
    }
}
