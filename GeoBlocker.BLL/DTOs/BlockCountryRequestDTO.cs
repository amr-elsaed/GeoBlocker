using System.ComponentModel.DataAnnotations;

namespace GeoBlocker.BLL.DTOs
{
    public class BlockCountryRequestDTO
    {
        [Required(ErrorMessage = "Country code is required.")]
        [StringLength(maximumLength: 3, MinimumLength = 2, ErrorMessage = "Country code must be exactly 2 or 3 characters.")]
        public string CountryCode { get; set; } = string.Empty;
    }
}
