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

namespace PhotoSi.Command.Product
{
    public class GetAllProductQuery : IQuery<List<ProductDto>>
    {
        public GetAllProductQuery()
        {
        }


        public class GetAllProductHandler : IRequestHandler<GetAllProductQuery, List<ProductDto>>
        {
            private readonly ILogger<GetAllProductHandler> _logger;
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public GetAllProductHandler(ILogger<GetAllProductHandler> logger,
                                        IProductRepository productRepository,
                                        IMapper mapper)
            {
                _logger = logger;
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<List<ProductDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
            {
                //Qua era meglio usare Dapper per fare una query ottimizzata per la get
                _logger.LogDebug("START");

                var products = await _productRepository.ListAllAsync();

                var productsDto = products?.Select(i => _mapper.Map<ProductDto>(i)).ToList();

                return productsDto;
            }

        }
    }
}
