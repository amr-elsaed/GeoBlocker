using GeoBlocker.BLL.Services.Abstraction;
using GeoBlocker.DAL.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace GeoBlocker.PL.Helper
{
    public class GeoIpService : IGeoIpService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogService _logService;

        public GeoIpService(IHttpClientFactory httpClientFactory, ILogger<GeoIpService> logger, IConfiguration configuration , ILogService logService)
        {
            _httpClient = httpClientFactory.CreateClient("IpApi");
            _configuration = configuration;
            _logService = logService;
        }

        public async Task<IpDetails?> GetIpDetailsAsync(string ipAddress)
        {
            try
            {
                var apiKey = _configuration["IpApiSettings:ApiKey"];
                var url = string.IsNullOrWhiteSpace(apiKey) ? $"{ipAddress}/json/" : $"{ipAddress}/json/?key={apiKey}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logService.AddLog($"IP API returned {response.StatusCode} for IP {ipAddress}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var details = JsonConvert.DeserializeObject<IpDetails>(content);

                if (details?.Error == true)
                {
                    _logService.AddLog($"IP API error for {ipAddress}: {details.Reason}");
                    return null;
                }

                return details;
            }
            catch (HttpRequestException ex)
            {
                _logService.AddLog($"HTTP request failed for IP {ipAddress} ex : {ex.Message}");
                return null;
            }
            catch (TaskCanceledException ex)
            {
                _logService.AddLog($"Request timed out for IP {ipAddress} ex : {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                _logService.AddLog($"Failed to deserialize IP API response for {ipAddress} ex : {ex.Message}");
                return null;
            }
        }

    }
}
