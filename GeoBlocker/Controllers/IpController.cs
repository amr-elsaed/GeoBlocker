using GeoBlocker.BLL.Services.Abstraction;
using GeoBlocker.DAL.Models;
using GeoBlocker.PL.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Net;

namespace GeoBlocker.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IGeoIpService _geoIpService;

        private readonly ICountryService countryService;
        private readonly ILogService _logger;


        public IpController(IGeoIpService geoIpService , ICountryService countryService, ILogService logger )
        {
            _geoIpService = geoIpService;
            this.countryService = countryService;
            _logger = logger;
        }
        [HttpGet("lookup")]
        [ProducesResponseType(typeof(IpDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress = null)
        {
            string resolvedIp;

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                resolvedIp = GetCallerIp();
                if (string.IsNullOrWhiteSpace(resolvedIp))
                    return BadRequest(new { message = "Could not determine caller IP address." });
            }
            else
            {
                if (!IsValidIpAddress(ipAddress))
                    return BadRequest(new { message = $"'{ipAddress}' is not a valid IP address format." });

                resolvedIp = ipAddress.Trim();
            }

            var details = await _geoIpService.GetIpDetailsAsync(resolvedIp);

            if (details == null)
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    new { message = "Could not retrieve IP details. The geolocation service may be unavailable." });

            return Ok(details);
        }

        [HttpGet("check-block")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CheckBlock()
        {
            var callerIp = GetCallerIp();
            if (string.IsNullOrWhiteSpace(callerIp))
                return BadRequest(new { message = "Could not determine caller IP address." });

            var userAgent = Request.Headers.UserAgent.ToString();

            var details = await _geoIpService.GetIpDetailsAsync(callerIp);

            if (details == null)
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    new { message = "Could not retrieve geolocation for your IP. Please try again later." });

            var countryCode = details.CountryCode ?? string.Empty;
            bool isBlocked = !string.IsNullOrEmpty(countryCode) && countryService.IsCountryBlocked(countryCode);

            // Log this attempt
            var log = new BlockedAttemptLog
            {
                IpAddress = callerIp,
                Timestamp = DateTime.UtcNow,
                CountryCode = countryCode,
                CountryName = details.CountryName ?? string.Empty,
                IsBlocked = isBlocked,
                UserAgent = userAgent
            };
            _logger.AddBlockedAttemptLog(log);
            _logger.AddLog($"Check-block for IP {callerIp}: country={countryCode}, blocked={isBlocked}");


            var responsePayload = new
            {
                ipAddress = callerIp,
                countryCode,
                countryName = details.CountryName,
                isBlocked,
                checkedAt = log.Timestamp
            };

            if (isBlocked)
                return StatusCode(StatusCodes.Status403Forbidden, responsePayload);

            return Ok(responsePayload);
        }



        private static bool IsValidIpAddress(string ip)
        {
            return IPAddress.TryParse(ip.Trim(), out _);
        }
        private string GetCallerIp()
        {
            var forwarded = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(forwarded))
            {
                var firstIp = forwarded.Split(',')[0].Trim();
                if (IsValidIpAddress(firstIp))
                    return firstIp;
            }

            var remoteIp = HttpContext.Connection.RemoteIpAddress;
            if (remoteIp == null)
                return string.Empty;

            if (IPAddress.IsLoopback(remoteIp))
                return "127.0.0.1";

            if (remoteIp.IsIPv4MappedToIPv6)
                return remoteIp.MapToIPv4().ToString();

            return remoteIp.ToString();
        }
    }
}
