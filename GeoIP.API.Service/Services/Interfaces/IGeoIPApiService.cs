using GeoIP.API.Service.Models;
using System.Threading.Tasks;

namespace Geo.API.Service.Services.Interfaces
{
    public interface IGeoIPApiService
    {
        Task<IpInfo> GetGeoInfoByIpAddress(string ipAddress);
    }
}