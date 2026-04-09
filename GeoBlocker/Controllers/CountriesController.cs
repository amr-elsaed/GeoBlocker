using GeoBlocker.BLL.DTOs;
using GeoBlocker.BLL.Services.Abstraction;
using GeoBlocker.BLL.Services.Implmentation;
using GeoBlocker.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace GeoBlocker.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService countryService;

        public CountriesController(ICountryService countryService)
        {
            this.countryService = countryService;
        }
        [HttpPost("block")]
        [ProducesResponseType(typeof(BlockedCountry), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> BlockCountry(BlockCountryRequestDTO blockCountry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var (IsSuccess, message, country) =await countryService.AddBlockedCountryAsync(blockCountry.CountryCode);
            if (!IsSuccess)
            {
                if (message.Contains("already"))
                {
                    return Conflict(new { message });
                }
                else
                {
                    return BadRequest(new { message });
                }
            }
            return CreatedAtAction(nameof(GetBlockedCountries), new { }, new
            {
                message,
                country
            });
        }


        [HttpGet("blocked")]
        [ProducesResponseType(typeof(PagedResult<BlockedCountry>), StatusCodes.Status200OK)]
        public IActionResult GetBlockedCountries([FromQuery] int page = 1,[FromQuery] int pageSize = 10,[FromQuery] string? search = null)
        {
            var result = countryService.GetBlockedCountries(page, pageSize, search);
            return Ok(result);
        }

        [HttpPost("temp-block")]
        [ProducesResponseType(typeof(TempBlockedCountry), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> TemporalBlock([FromBody] TempBlockRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, block) = await countryService.AddTempBlockAsync(request.CountryCode, request.DurationMinutes);

            if (!success)
            {
                if (message.Contains("already"))
                {
                    return Conflict(new { message });
                }
                return BadRequest(new { message });
            }
            return StatusCode(201, new { message, block });
        }


        [HttpDelete("block/{countryCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UnblockCountry([FromRoute] string countryCode)
        {
            var (success, message) = countryService.RemoveBlockedCountry(countryCode);

            if (!success)
            {
                return NotFound(new { message });
            }
            return Ok(new { message });
        }

        [HttpGet("temporal-blocks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTemporalBlocks()
        {
            var blocks = countryService.GetAllTempBlock();

            return Ok(new
            {
                count = blocks.Count,
                blocks
            });
        }
    }
}
