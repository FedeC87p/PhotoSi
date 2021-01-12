using AutoMapper;
using DomainModel.Dtos;
using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Interfaces.Mediator;
using PhotoSi.Query.Product.QueryResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Query.Options
{
    public class GetAllOptionQuery : IQuery<List<OptionDto>>
    {
        public GetAllOptionQuery()
        {
        }


        public class GetAllOptionHandler : IRequestHandler<GetAllOptionQuery, List<OptionDto>>
        {
            private readonly ILogger<GetAllOptionHandler> _logger;
            private readonly IRepository<Option> _optionRepository;
            private readonly IMapper _mapper;

            public GetAllOptionHandler(ILogger<GetAllOptionHandler> logger,
                                        IRepository<Option> optionRepository,
                                        IMapper mapper)
            {
                _logger = logger;
                _optionRepository = optionRepository;
                _mapper = mapper;
            }

            public async Task<List<OptionDto>> Handle(GetAllOptionQuery request, CancellationToken cancellationToken)
            {
                //Qua era meglio usare Dapper per fare una query ottimizzata per la get
                _logger.LogDebug("START");

                var options = await _optionRepository.ListAllAsync();

                var optionsDto = options?.Select(i => _mapper.Map<OptionDto>(i)).ToList();

                return optionsDto;
            }

        }
    }
}
