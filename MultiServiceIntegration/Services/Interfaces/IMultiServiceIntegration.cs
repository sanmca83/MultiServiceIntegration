using MultiServiceIntegration.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MultiServiceIntegration.Services.Interfaces
{
    public interface IMultiServiceIntegration
    {
        bool ValidateIPAddress(string ipAddress);
        //Task<object> CallAPI(string url, HttpClient httpClient);
        Task<List<IpInfo>> CallApiUsingAsyncAwait(string ipAddress, List<string> services, HttpClient httpClient);
        Task<IpInfo> CallAPIUsingTPI(string serviceName, string ipAddress, HttpClient httpClient);

    }
}   