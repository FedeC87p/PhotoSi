using AutoMapper;
using DomainModel.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Categories;
using PhotoSi.Command.Product;
using PhotoSi.Interfaces.Mediator;
using PhotoSi.Query.Categories;
using PhotoSi.Query.Product.QueryResult;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.ModelViews.Request;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ApiBaseController
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMapper _mapper;

        public CategoriesController(ILogger<CategoriesController> logger,
            IMediatorService mediatorService,
            IMapper mapper)
            : base(mediatorService)
        {
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        ///     Get category.
        /// </summary>
        /// <response code="201">Get category</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns></returns>
        [HttpGet("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            _logger.LogDebug("START GetCategory");
            var categoryResult = await QueryAsync(new GetCategoryByIdQuery
            {
                CategoryId = categoryId
            });

            if (categoryResult != null)
            {
                return Ok(categoryResult);
            }
            else
            {
                return NotFound($"Category id {categoryId} not found");
            }
        }

        /// <summary>
        ///     Get categories.
        /// </summary>
        /// <response code="200">Get categories</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {
            _logger.LogDebug("START GetCategories");
            var categories = await QueryAsync(new GetAllCategoryQuery());

            return Ok(categories);
        }

        /// <summary>
        ///     Create new category.
        /// </summary>
        /// <response code="201">Create product</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] CategoryCreateRequest category)
        {
            _logger.LogDebug("START CreateNode");
            var categoryResult = await CommandAsync(new CreateCategoryCommand
            {
                Category = _mapper.Map<CategoryDto>(category)
            });

            if (!categoryResult.HaveError)
            {
                return Created($"categories/{categoryResult.CategoryId.Value}", categoryResult.CategoryId.Value);
            }
            else
            {
                return UnprocessableEntity(categoryResult.Errors);
            }
        }

        /// <summary>
        ///     Update category.
        /// </summary>
        /// <response code="204">Edit category</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns></returns>
        [HttpPut("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateRequest category, int categoryId)
        {
            _logger.LogDebug("START UpdateCategory");

            var categoryDto = _mapper.Map<CategoryDto>(category);
            categoryDto.CategoryId = categoryId;
            var productResult = await CommandAsync(new UpdateCategoryCommand
            {
                Category = categoryDto
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
        ///     Delete category.
        /// </summary>
        /// <response code="204">Delete category</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns></returns>
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int categoryId)
        {
            _logger.LogDebug("START UpdateProduct");
            var categoryResult = await CommandAsync(new DeleteCategoryCommand
            {
                CategoryId = categoryId
            });

            if (!categoryResult.HaveError)
            {
                return NoContent();
            }
            else
            {
                return UnprocessableEntity(categoryResult.Errors);
            }
        }
    }
}
