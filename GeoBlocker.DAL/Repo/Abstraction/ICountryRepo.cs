using GeoBlocker.DAL.Models;

namespace GeoBlocker.DAL.Repo.Abstraction
{
    public interface ICountryRepo
    {
        public bool AddBlockedCountry(BlockedCountry country);
        public bool RemoveBlockedCountry(string CountryCode);
        public BlockedCountry? GetBlockedCountry(string CountryCode);
        public List<BlockedCountry> GetBlockedCountries();
        public bool IsBlocked(string CountryCode);
        public bool AddTempBlock(TempBlockedCountry country);
        public bool RemoveTemporalBlock(string CountryName);
        public TempBlockedCountry? GetTempBlockedCountry(string CountryName);
        public List<TempBlockedCountry> GetAllTempBlockedCountries();
        public bool IsTempBlocked(string CountryName);
        public void RemoveExpiredTemporalBlocks();
    }
}
