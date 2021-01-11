using AutoMapper;
using DomainModel.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Product;
using PhotoSi.Interfaces;
using PhotoSi.Interfaces.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ModelViews.Request;

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
        ///     Create new node.
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
                return Created($"productId", productResult.ProductId.Value);
            }
            else
            {
                return UnprocessableEntity(productResult.Errors);
            }
        }
    }
}
