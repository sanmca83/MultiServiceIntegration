using Microsoft.Extensions.Logging;
using MultiServiceIntegration.Data.Enumerations;
using MultiServiceIntegration.Models;
using MultiServiceIntegration.Services.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MultiServiceIntegration.Services
{
    public class MultiServiceIntegration : IMultiServiceIntegration
    {
        private readonly ILogger<MultiServiceIntegration> _logger;
        public MultiServiceIntegration(ILogger<MultiServiceIntegration> logger)
        {
            _logger = logger;
        }
        public bool ValidateIPAddress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return false;
            }

            string[] splitVlauesByDot = ipAddress.Split('.');
            if (splitVlauesByDot.Length != 4)
            {
                return false;
            }

            return splitVlauesByDot.All(r => byte.TryParse(r, out byte ip));
        }

        public async Task<object> CallAPI(string url, HttpClient httpClient)
        {
            return await httpClient.GetStringAsync(url);
        }

        public async Task<List<IpInfo>> CallApiUsingAsyncAwait(string ipAddress, List<string> services, HttpClient httpClient)
        {
            List<IpInfo> ipInfos = new List<IpInfo>();
            if (services.Any(s => s.ToUpper() == ServiceTypes.RDAPService))
            {
                var httpUrl = $"{Startup.Configuration["RDAP_API_URL"]}?ipAddress={ipAddress}";
                _logger.LogInformation($"Call RDAP service to get the information. URI {httpUrl}");
                var response = await CallAPI(httpUrl, httpClient);
                var ipInfo = JsonConvert.DeserializeObject<IpInfo>(response.ToString());
                ipInfo.ServiceName = ServiceTypes.RDAPService;
                ipInfos.Add(ipInfo);
            }

            if (services.Any(s => s.ToUpper() == ServiceTypes.GeoService))
            {
                var httpUrl = $"{Startup.Configuration["GEOIP_API_URL"]}?ipAddress={ipAddress}";
                _logger.LogInformation($"Call GEOIP service to get the information. URI {httpUrl}");
                var response = await CallAPI(httpUrl, httpClient);
                var ipInfo = JsonConvert.DeserializeObject<IpInfo>(response.ToString());
                ipInfo.ServiceName = ServiceTypes.GeoService;
                ipInfos.Add(ipInfo);
            }
            return ipInfos;
        }

        public async Task<IpInfo> CallAPIUsingTPI(string serviceName, string ipAddress, HttpClient httpClient)
        {
            IpInfo ipInfo = new IpInfo();
            if (serviceName == ServiceTypes.RDAPService)
            {
                var httpUrl = $"{Startup.Configuration["RDAP_API_URL"]}?ipAddress={ipAddress}";
                var response = await CallAPI(httpUrl, httpClient);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(response.ToString());
                ipInfo.ServiceName = ServiceTypes.RDAPService;
            }

            if (serviceName == ServiceTypes.GeoService)
            {
                var httpUrl = $"{Startup.Configuration["GEOIP_API_URL"]}?ipAddress={ipAddress}";
                var response = await CallAPI(httpUrl, httpClient);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(response.ToString());
                ipInfo.ServiceName = ServiceTypes.GeoService;
            }
            return ipInfo;
        }
    }
}
