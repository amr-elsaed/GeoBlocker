using GeoBlocker.DAL.Models;

namespace GeoBlocker.PL.Helper
{
    public interface IGeoIpService
    {
        Task<IpDetails?> GetIpDetailsAsync(string ipAddress);
    }
}
