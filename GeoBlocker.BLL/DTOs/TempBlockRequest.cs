using System.ComponentModel.DataAnnotations;

namespace GeoBlocker.BLL.DTOs
{
    public class TempBlockRequest
    {
        [Required(ErrorMessage = "Country code is required.")]
        [StringLength(maximumLength:3, MinimumLength = 2, ErrorMessage = "Country code must be exactly 2 or 3 chars.")]
        public string CountryCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Duration in minutes is required.")]
        [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes.")]
        public int DurationMinutes { get; set; }
    }
}
