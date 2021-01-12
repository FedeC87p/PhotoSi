using AutoMapper;
using DomainModel.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Product;
using PhotoSi.Interfaces.Mediator;
using PhotoSi.Query.Product.QueryResult;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.ModelViews.Request;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ApiBaseController
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public ProductsController(ILogger<ProductsController> logger,
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
        ///     Get product.
        /// </summary>
        /// <response code="201">Create node</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns>JsonSdmx</returns>
        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModelView))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProduct(int productId)
        {
            _logger.LogDebug("START UpdateProduct");
            var productResult = await QueryAsync(new GetProductByIdQuery
            {
                ProductId = productId
            });

            if (productResult != null)
            {
                return Ok(productResult);
            }
            else
            {
                return NotFound($"Product id {productId} not found");
            }
        }

        /// <summary>
        ///     Get products.
        /// </summary>
        /// <response code="201">Create node</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns>JsonSdmx</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProducts()
        {
            _logger.LogDebug("START UpdateProduct");
            var productResult = await QueryAsync(new GetAllProductQuery());

            return Ok(productResult);
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
                return Created($"products/{productResult.ProductId.Value}", productResult.ProductId.Value);
            }
            else
            {
                return UnprocessableEntity(productResult.Errors);
            }
        }

        /// <summary>
        ///     Update new product.
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
