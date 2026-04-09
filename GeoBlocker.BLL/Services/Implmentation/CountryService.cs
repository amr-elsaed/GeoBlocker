using GeoBlocker.BLL.DTOs;
using GeoBlocker.BLL.Services.Abstraction;
using GeoBlocker.DAL.Models;
using GeoBlocker.DAL.Repo.Abstraction;

namespace GeoBlocker.BLL.Services.Implmentation
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepo countryRepo;
        private readonly ILogService logService;
        public CountryService(ICountryRepo countryRepo , ILogService logService)
        {
            this.countryRepo = countryRepo;
            this.logService = logService;
        }
        public Task<(bool IsSuccess, string msg, BlockedCountry? blockedCountry)> AddBlockedCountryAsync(string CountryCode)
        {
            var code = CountryCode.Trim().ToUpperInvariant();

            if (countryRepo.IsBlocked(code))
            {
                return Task.FromResult <(bool, string, BlockedCountry?)>((false, $"Country '{code}' is already in the permanent blocked list", null));
            }
            if (countryRepo.IsTempBlocked(code))
            {
                return Task.FromResult<(bool, string, BlockedCountry?)>((false, $"Country '{code}' is already in the Temp blocked list", null));
            }
            var Country = new BlockedCountry()
            {
                CountryCode = code,
                AddedTime = DateTime.Now
            };
            bool IsAdded = countryRepo.AddBlockedCountry(Country);
            if (!IsAdded)
            {
                return Task.FromResult <(bool, string, BlockedCountry?)>((false, $"Country '{code}' could not be added , try again later", null));
            }
            logService.AddLog($"Country '{code}' added to permanent blocked list.");
            return Task.FromResult<(bool, string, BlockedCountry?)>((true, $"Country '{code}' has been blocked successfully.", Country));
        }
        public (bool Success, string Message) RemoveBlockedCountry(string countryCode)
        {
            var code = countryCode.Trim().ToUpperInvariant();
            bool removed = countryRepo.RemoveBlockedCountry(code);
            if (!removed)
            {
                return (false, $"Country {code} was not found in the blocked list");
            }
            logService.AddLog($"Country '{code}' removed from permanent blocked list.");
            return (true, $"Country '{code}' has been unblocked successfully.");
        }
        public PagedResult<BlockedCountry> GetBlockedCountries(int page, int pageSize, string? search)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var AllBlockedCountries = countryRepo.GetBlockedCountries();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var Search = search.Trim().ToUpperInvariant();
                AllBlockedCountries = AllBlockedCountries.Where(c => c.CountryCode.Contains(Search)).ToList();
            }

            var sortedCounties = AllBlockedCountries.OrderBy(c => c.CountryCode).ToList();
            var total = sortedCounties.Count;
            var items = sortedCounties.Skip((page-1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<BlockedCountry>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }
        public Task<(bool Success, string Message, TempBlockedCountry? Block)> AddTempBlockAsync(string countryCode, int ExpDate)
        {
            var code = countryCode.Trim().ToUpperInvariant();

            if (ExpDate < 1 || ExpDate > 1440)
            {
                return Task.FromResult<(bool, string, TempBlockedCountry?)>((false, "Expiration Date Must be between 1 and 1440 minutes", null));
            }
            if (countryRepo.IsTempBlocked(code))
            {
                return Task.FromResult<(bool, string, TempBlockedCountry?)>((false, $"Country '{code}' is already permanently blocked", null));
            }
            var IsAlreadyExist = countryRepo.GetTempBlockedCountry(code);
            if (IsAlreadyExist != null && !IsAlreadyExist.IsExpires)
            {
                return Task.FromResult<(bool, string, TempBlockedCountry?)>((false, $"Country '{code}' is already temp blocked until {IsAlreadyExist.ExpiresAt}", null));
            }
            //if expired and wait for the backgrounf service
            if (IsAlreadyExist != null &&  IsAlreadyExist.IsExpires)
            {
                countryRepo.RemoveTemporalBlock(code);
            }
            var block = new TempBlockedCountry
            {
                CountryCode = code,
                DeletedTime = DateTime.Now,
                ExpiresAt = DateTime.Now.AddMinutes(ExpDate)
            };
            bool IsAdd = countryRepo.AddTempBlock(block);
            if (!IsAdd)
            {
                return Task.FromResult<(bool, string, TempBlockedCountry?)>((false, $"Country '{code}' could not be temp blocked , please try again later", null));
            }
            logService.AddLog($"Country '{code}' temporarily blocked for {ExpDate} minutes");
            return Task.FromResult<(bool, string, TempBlockedCountry?)>((true, $"Country '{code}' has been temporarily blocked for {ExpDate} minutes adn will expires at {block.ExpiresAt}", block));
        }
        public bool IsCountryBlocked(string countryCode)
        {
            var code = countryCode.Trim().ToUpperInvariant();
            return countryRepo.IsBlocked(code) || countryRepo.IsTempBlocked(code);
        }
        public List<TempBlockedCountry> GetAllTempBlock()
        {
            var AllTempBlockCountries = countryRepo.GetAllTempBlockedCountries();
            return AllTempBlockCountries.ToList();
        }
    }
}
