using AutoMapper;
using DomainModel.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoSi.Command.Options;
using PhotoSi.Interfaces.Mediator;
using PhotoSi.Query.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.ModelViews.Request;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OptionsController : ApiBaseController
    {
        private readonly ILogger<OptionsController> _logger;
        private readonly IMapper _mapper;

        public OptionsController(ILogger<OptionsController> logger,
            IMediatorService mediatorService,
            IMapper mapper)
            : base(mediatorService)
        {
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        ///     Get option.
        /// </summary>
        /// <response code="201">Get option</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns></returns>
        [HttpGet("{optionId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OptionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOption(int optionId)
        {
            _logger.LogDebug("START GetOption");
            var optionResult = await QueryAsync(new GetOptionByIdQuery
            {
                OptionId = optionId
            });

            if (optionResult != null)
            {
                return Ok(optionResult);
            }
            else
            {
                return NotFound($"Option id {optionId} not found");
            }
        }

        /// <summary>
        ///     Get options.
        /// </summary>
        /// <response code="200">Get options</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OptionDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOptions()
        {
            _logger.LogDebug("START GetOptions");
            var options = await QueryAsync(new GetAllOptionQuery());

            return Ok(options);
        }

        /// <summary>
        ///     Create new option.
        /// </summary>
        /// <response code="201">Create product</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOption([FromBody] OptionCreateRequest option)
        {
            _logger.LogDebug("START CreateOption");
            var optionResult = await CommandAsync(new CreateOptionCommand
            {
                Option = _mapper.Map<OptionDto>(option)
            });

            if (!optionResult.HaveError)
            {
                return Created($"oiptions/{optionResult.OptionId.Value}", optionResult.OptionId.Value);
            }
            else
            {
                return UnprocessableEntity(optionResult.Errors);
            }
        }

        /// <summary>
        ///     Update option.
        /// </summary>
        /// <response code="204">Edit option</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns></returns>
        [HttpPut("{optionId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOption([FromBody] OptionUpdateRequest option, int optionId)
        {
            _logger.LogDebug("START UpdateOption");

            if (optionId != option.OptionId)
            {
                return BadRequest($"invalid optionId");
            }

            var optionDto = _mapper.Map<OptionDto>(option);
            optionDto.OptionId = optionId;
            var productResult = await CommandAsync(new UpdateOptionCommand
            {
                Option = optionDto
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
        ///     Delete option.
        /// </summary>
        /// <response code="204">Delete option</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <returns></returns>
        [HttpDelete("{optionId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int optionId)
        {
            _logger.LogDebug("START UpdateProduct");
            var optionResult = await CommandAsync(new DeleteOptionCommand
            {
                OptionId = optionId
            });

            if (!optionResult.HaveError)
            {
                return NoContent();
            }
            else
            {
                return UnprocessableEntity(optionResult.Errors);
            }
        }
    }
}
