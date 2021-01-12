using AutoMapper;
using DomainModel.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Orders;
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
    public class OrdersController : ApiBaseController
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(ILogger<OrdersController> logger,
            IMediatorService mediatorService,
            IMapper mapper)
            : base(mediatorService)
        {
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        ///     Create new order.
        /// </summary>
        /// <response code="201">Create product</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] OrderCreateRequest order)
        {
            _logger.LogDebug("START CreateProduct");
            var productResult = await CommandAsync(new CreateOrderCommand
            {
                Order = _mapper.Map<OrderDto>(order)
            });

            if (!productResult.HaveError)
            {
                return Created($"orders/{productResult.OrderId.Value}", productResult.OrderId.Value);
            }
            else
            {
                return UnprocessableEntity(productResult.Errors);
            }
        }

    }
}
