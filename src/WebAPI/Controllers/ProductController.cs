using AutoMapper;
using DomainModel.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Product;
using PhotoSi.Interfaces.Mediator;
using System;
using System.Threading.Tasks;
using WebAPI.ModelViews.Request;
using WebAPI.ModelViews.Response;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ApiBaseController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public ProductController(ILogger<ProductController> logger,
            IMediatorService mediatorService,
            IServiceProvider serviceProvider,
            IMapper mapper)
            : base(mediatorService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        /// <summary>
        ///     Create new product.
        /// </summary>
        /// <response code="201">Create node</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns>JsonSdmx</returns>
        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModelView))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            _logger.LogDebug("START UpdateProduct");
            var productResult = await QueryAsync(new DeleteProductQuery
            {
                ProductId = productId
            });

            if (!productResult.HaveError)
            {
                return NoContent();
            }
            else
            {
                return UnprocessableEntity(productResult.Errors);
            }
        }

        /// <summary>
        ///     Create new product.
        /// </summary>
        /// <response code="201">Create node</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns>JsonSdmx</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest product)
        {
            _logger.LogDebug("START CreateNode");
            var productResult = await CommandAsync(new CreateProductCommand
            {
                Product = _mapper.Map<ProductDto>(product)
            });

            if (!productResult.HaveError)
            {
                return Created($"product/{productResult.ProductId.Value}", productResult.ProductId.Value);
            }
            else
            {
                return UnprocessableEntity(productResult.Errors);
            }
        }

        /// <summary>
        ///     Create new product.
        /// </summary>
        /// <response code="201">Create node</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns>JsonSdmx</returns>
        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateRequest product, int productId)
        {
            _logger.LogDebug("START UpdateProduct");

            var productDto = _mapper.Map<ProductDto>(product);
            productDto.ProductId = productId;
            var productResult = await CommandAsync(new UpdateProductCommand
            {
                Product = productDto
            });

            if (!productResult.HaveError)
            {
                return NoContent();
            }
            else
            {
                return UnprocessableEntity(productResult.Errors);
            }
        }

        /// <summary>
        ///     Create new product.
        /// </summary>
        /// <response code="201">Create node</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns>JsonSdmx</returns>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            _logger.LogDebug("START UpdateProduct");
            var productResult = await CommandAsync(new DeleteProductCommand
            {
                ProductId = productId
            });

            if (!productResult.HaveError)
            {
                return NoContent();
            }
            else
            {
                return UnprocessableEntity(productResult.Errors);
            }
        }
    }
}
