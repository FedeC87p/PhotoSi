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
    public class GetProductByIdQuery : IQuery<ProductModelView>
    {
        public GetProductByIdQuery()
        {
            IncludeOptionName = true;
        }

        public int ProductId { get; set; }
        public bool IncludeOptionName { get; set; }

        public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductModelView>
        {
            private readonly ILogger<GetProductByIdHandler> _logger;
            private readonly IProductRepository _productRepository;
            private readonly IRepository<Option> _optionRepository;
            private readonly IRepository<Category> _categorytRepository;
            private readonly IMapper _mapper;

            public GetProductByIdHandler(ILogger<GetProductByIdHandler> logger,
                                        IProductRepository productRepository,
                                        IRepository<Option> optionRepository,
                                        IRepository<Category> categorytRepository,
                                        IMapper mapper)
            {
                _logger = logger;
                _productRepository = productRepository;
                _optionRepository = optionRepository;
                _categorytRepository = categorytRepository;
                _mapper = mapper;
            }

            public async Task<ProductModelView> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                //Qua era meglio usare Dapper per fare una query ottimizzata per la get
                _logger.LogDebug("START");

                var product = await _productRepository.GetByIdAsync(request.ProductId);

                if (product == null)
                {
                    return null;
                }

                var category = await _categorytRepository.GetByIdAsync(product.CategoryId);//gestire eventuale null

                var productDto = _mapper.Map<ProductDto>(product);
                var categoryDto = _mapper.Map<CategoryDto>(category);

                Dictionary<int, string> optionDto;
                if (request.IncludeOptionName &&
                    product?.Options != null)
                {
                    optionDto = new Dictionary<int, string>();
                    foreach (var item in product?.Options)
                    { //gestire eventuale null
                        var entity = await _optionRepository.GetByIdAsync(item.OptionId); //Qui era meglio recuperare tutti gli Id, anzichè uno alla volta. Magari con lo specification pattern 
                        optionDto.Add(entity.OptionId, entity.Name);
                    }
                }
                else
                {
                    optionDto = product?.Options?.ToDictionary(i => i.OptionId, i => "");
                }

                return new ProductModelView
                {
                    CategoryId = categoryDto.CategoryId,
                    CategoryName = categoryDto.Name,
                    ProductId = productDto.ProductId,
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Note = productDto.Note,
                    Options = optionDto
                };
            }

        }
    }
}
