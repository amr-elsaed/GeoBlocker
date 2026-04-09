namespace GeoBlocker.DAL.Models
{
    public class TempBlockedCountry
    {
        public string CountryCode { get; set; } = string.Empty;
        public DateTime DeletedTime { get; set; } = DateTime.Now;
        public DateTime ExpiresAt {  get; set; }

        //Function for auto test weather expires or not
        public bool IsExpires => DateTime.Now >= ExpiresAt;
    }
}
