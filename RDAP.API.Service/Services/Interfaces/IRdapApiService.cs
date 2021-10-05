using RDAP.API.Service.Models;
using System.Threading.Tasks;

namespace RDAP.API.Service.Services.Interfaces
{
    public interface IRdapApiService
    {
        Task<IpInfo> GetRdapInfoByIpAddress(string ipAddress);
    }
}