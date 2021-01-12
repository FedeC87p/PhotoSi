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
    public class GetCategoryByIdQuery : IQuery<CategoryDto>
    {
        public GetCategoryByIdQuery()
        {
        }

        public int CategoryId { get; set; }

        public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
        {
            private readonly ILogger<GetCategoryByIdHandler> _logger;
            private readonly IRepository<Category> _categoryRepository;
            private readonly IMapper _mapper;

            public GetCategoryByIdHandler(ILogger<GetCategoryByIdHandler> logger,
                                        IRepository<Category> categoryRepository,
                                        IMapper mapper)
            {
                _logger = logger;
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }

            public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                //Qua era meglio usare Dapper per fare una query ottimizzata per la get
                _logger.LogDebug("START");

                var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

                if (category == null)
                {
                    return null;
                }

                return _mapper.Map<CategoryDto>(category);
            }

        }
    }
}
