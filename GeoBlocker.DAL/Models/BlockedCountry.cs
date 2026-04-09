namespace GeoBlocker.DAL.Models
{
    public class BlockedCountry
    {
        public string CountryCode { get; set; } = string.Empty;
        public DateTime AddedTime { get; set; } = DateTime.Now;
    }
}
