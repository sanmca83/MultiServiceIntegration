using RDAP.API.Service.Models;
using RDAP.API.Service.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RDAP.API.Service.Services
{
    public class RdapApiService : IRdapApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RdapApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Takes ipAddress and calls RDAP Api
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns>Returns IpInfo</returns>
        public async Task<IpInfo> GetRdapInfoByIpAddress(string ipAddress)
        {
            IpInfo ipInfo;
            try
            {
                string rdpApiuRL = Startup.Configuration["RDP_API_URL"];
                var httpUrl = $"{rdpApiuRL}{ipAddress}";
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(httpUrl, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    var rdapContent = await JsonSerializer.DeserializeAsync<object>(stream);
                    ipInfo = GetMessage(true, rdapContent.ToString(), response.StatusCode.ToString());
                }
                else
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    ipInfo = GetMessage(false, response.ToString(), response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                ipInfo = GetMessage(false, ex?.Message.ToString(), HttpStatusCode.InternalServerError.ToString());
            }

            return ipInfo;
        }

        private IpInfo GetMessage(bool isSuccess, string response, string statusCode)
        {
            return new IpInfo
            {
                IsSuccess = isSuccess,
                Response = response,
                StatusCode = statusCode
            };
        }
    }
}
