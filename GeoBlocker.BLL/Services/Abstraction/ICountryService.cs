using GeoBlocker.BLL.DTOs;
using GeoBlocker.DAL.Models;

namespace GeoBlocker.BLL.Services.Abstraction
{
    public interface ICountryService
    {
        public Task<(bool IsSuccess, string msg, BlockedCountry? blockedCountry)> AddBlockedCountryAsync(string CountryCode);
        public (bool Success, string Message) RemoveBlockedCountry(string countryCode);
        public PagedResult<BlockedCountry> GetBlockedCountries(int page, int pageSize, string? search);
        public Task<(bool Success, string Message, TempBlockedCountry? Block)> AddTempBlockAsync(string countryCode, int ExpDate);
        public List<TempBlockedCountry> GetAllTempBlock();

        public bool IsCountryBlocked(string countryCode);
    }
}
