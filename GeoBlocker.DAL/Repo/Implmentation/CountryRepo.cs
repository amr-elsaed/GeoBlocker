using GeoBlocker.DAL.Repo.Abstraction;

namespace GeoBlocker.DAL.Repo.Implmentation
{
    public class CountryRepo : ICountryRepo
    {
        private readonly ConcurrentDictionary<string , BlockedCountry> _BlockedCountries = new ConcurrentDictionary<string , BlockedCountry>();
        private readonly ConcurrentDictionary<string , TempBlockedCountry> _TempBlockedCountry = new ConcurrentDictionary<string , TempBlockedCountry>();

        
        #region NormalBlockedCountries
        //Adding new Blocked Country to Dict
        public bool AddBlockedCountry(BlockedCountry country)
        {
            var key = country.CountryCode.ToUpperInvariant();
            return _BlockedCountries.TryAdd(key,country);
        }

        //Remove From Blocked Country
        public bool RemoveBlockedCountry(string CountryCode)
        {
            return _BlockedCountries.TryRemove(CountryCode.ToUpperInvariant() , out var val); 
        }

        //Search in Blocked Countries
        public BlockedCountry? GetBlockedCountry(string CountryCode)
        {
            _BlockedCountries.TryGetValue(CountryCode.ToUpperInvariant(), out var val);
            return val;
        }

        //Get All Blocked Countries
        public List<BlockedCountry> GetBlockedCountries()
        {
            return _BlockedCountries.Values.ToList();
        }

        //Check if blocked
        public bool IsBlocked(string CountryCode)
        {
            return _BlockedCountries.ContainsKey(CountryCode.ToUpperInvariant());
        }
        #endregion


        #region TempBlockedCountries
        public bool AddTempBlock(TempBlockedCountry country)
        {
            return _TempBlockedCountry.TryAdd(country.CountryCode.ToUpperInvariant(), country);
        }
        public bool RemoveTemporalBlock(string CountryName)
        {
            return _TempBlockedCountry.TryRemove(CountryName.ToUpperInvariant(), out var val);
        }
        public TempBlockedCountry? GetTempBlockedCountry(string CountryName)
        {
            _TempBlockedCountry.TryGetValue(CountryName.ToUpperInvariant(), out var val);
            return val;
        }
        public List<TempBlockedCountry> GetAllTempBlockedCountries()
        {
            return _TempBlockedCountry.Values.ToList();
        }
        public bool IsTempBlocked(string CountryName)
        {
            var key = CountryName.ToUpperInvariant();
            if(_TempBlockedCountry.TryGetValue(key , out var val))
            {
                if (!val.IsExpires)
                {
                    return true;
                }
                _TempBlockedCountry.TryRemove(key, out _);
            }
            return false;
        }
        public void RemoveExpiredTemporalBlocks()
        {
            var ExpiredKeys = _TempBlockedCountry.Where(x=>x.Value.IsExpires).Select(x=>x.Key).ToList();
            foreach(var key in ExpiredKeys)
            {
                _TempBlockedCountry.TryRemove(key,out _);
            }
        }
        #endregion
    }
}
