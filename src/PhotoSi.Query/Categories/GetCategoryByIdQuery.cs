using AutoMapper;
using DomainModel.Dtos;
using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Interfaces.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Query.Categories
{
    public class GetCategoryByIdQuery : IQuery<OptionDto>
    {
        public GetCategoryByIdQuery()
        {
        }

        public int ProductId { get; set; }

        public class GetOptionByIdHandler : IRequestHandler<GetCategoryByIdQuery, OptionDto>
        {
            private readonly ILogger<GetOptionByIdHandler> _logger;
            private readonly IRepository<Option> _optionRepository;
            private readonly IMapper _mapper;

            public GetOptionByIdHandler(ILogger<GetOptionByIdHandler> logger,
                                        IRepository<Option> optionRepository,
                                        IMapper mapper)
            {
                _logger = logger;
                _optionRepository = optionRepository;
                _mapper = mapper;
            }

            public async Task<OptionDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                //Qua era meglio usare Dapper per fare una query ottimizzata per la get
                _logger.LogDebug("START");

                var option = await _optionRepository.GetByIdAsync(request.ProductId);

                if (option == null)
                {
                    return null;
                }

                return _mapper.Map<OptionDto>(option);
            }

        }
    }
}
