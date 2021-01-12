using AutoMapper;
using DomainModel.Dtos;
using DomainModel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Interfaces.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Command.Product
{
    public class GetProductByIdCommand : ICommand<ProductDto>
    {
        public GetProductByIdCommand()
        {
        }

        public int ProductId { get; set; }

        public class GetProductByIdHandler : IRequestHandler<GetProductByIdCommand, ProductDto>
        {
            private readonly ILogger<GetProductByIdHandler> _logger;
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public GetProductByIdHandler(ILogger<GetProductByIdHandler> logger,
                                        IProductRepository productRepository,
                                        IMapper mapper)
            {
                _logger = logger;
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<ProductDto> Handle(GetProductByIdCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("START");

                var product = await _productRepository.GetByIdAsync(request.ProductId);

                return _mapper.Map<ProductDto>(product);
            }

        }
    }
}
