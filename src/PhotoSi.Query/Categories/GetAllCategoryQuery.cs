using AutoMapper;
using DomainModel.Dtos;
using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Interfaces.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Query.Categories
{
    public class GetAllCategoryQuery : IQuery<List<CategoryDto>>
    {
        public GetAllCategoryQuery()
        {
        }


        public class GetAllCategoryHandler : IRequestHandler<GetAllCategoryQuery, List<CategoryDto>>
        {
            private readonly ILogger<GetAllCategoryHandler> _logger;
            private readonly IRepository<Category> _categoryRepository;
            private readonly IMapper _mapper;

            public GetAllCategoryHandler(ILogger<GetAllCategoryHandler> logger,
                                        IRepository<Category> categoryRepository,
                                        IMapper mapper)
            {
                _logger = logger;
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }

            public async Task<List<CategoryDto>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
            {
                //Qua era meglio usare Dapper per fare una query ottimizzata per la get
                _logger.LogDebug("START");

                var categories = await _categoryRepository.ListAllAsync();

                var categoriesDto = categories?.Select(i => _mapper.Map<CategoryDto>(i)).ToList();

                return categoriesDto;
            }

        }
    }
}
