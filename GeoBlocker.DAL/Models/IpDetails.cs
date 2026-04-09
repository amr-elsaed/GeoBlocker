namespace GeoBlocker.DAL.Models
{
    public class IpDetails
    {
        [JsonProperty("ip")]
        public string Ip { get; set; } = string.Empty;

        [JsonProperty("version")]
        public string Version { get; set; } = string.Empty;

        [JsonProperty("city")]
        public string City { get; set; } = string.Empty;

        [JsonProperty("region")]
        public string Region { get; set; } = string.Empty;

        [JsonProperty("region_code")]
        public string RegionCode { get; set; } = string.Empty;

        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        [JsonProperty("country_name")]
        public string CountryName { get; set; } = string.Empty;

        [JsonProperty("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        [JsonProperty("country_code_iso3")]
        public string CountryCodeIso3 { get; set; } = string.Empty;

        [JsonProperty("country_capital")]
        public string CountryCapital { get; set; } = string.Empty;

        [JsonProperty("continent_code")]
        public string ContinentCode { get; set; } = string.Empty;

        [JsonProperty("in_eu")]
        public bool InEu { get; set; }

        [JsonProperty("postal")]
        public string Postal { get; set; } = string.Empty;

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; } = string.Empty;

        [JsonProperty("utc_offset")]
        public string UtcOffset { get; set; } = string.Empty;

        [JsonProperty("country_calling_code")]
        public string CountryCallingCode { get; set; } = string.Empty;

        [JsonProperty("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonProperty("currency_name")]
        public string CurrencyName { get; set; } = string.Empty;

        [JsonProperty("languages")]
        public string Languages { get; set; } = string.Empty;

        [JsonProperty("asn")]
        public string Asn { get; set; } = string.Empty;

        [JsonProperty("org")]
        public string Org { get; set; } = string.Empty;

        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("reason")]
        public string? Reason { get; set; }
    }
}
