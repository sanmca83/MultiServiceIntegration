using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MultiServiceIntegration.Data.Enumerations;
using MultiServiceIntegration.Models;
using MultiServiceIntegration.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MultiServiceIntegration.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MultiServiceController : ControllerBase
    {
        private readonly ILogger<MultiServiceController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IMultiServiceIntegration _multiSvcIntg;

        public MultiServiceController(ILogger<MultiServiceController> logger, IMultiServiceIntegration multiSvcIntg,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _multiSvcIntg = multiSvcIntg;
            _httpClient = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Call servies using async and await
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAggegrateResultsUsingAysncAwait")]
        public async Task<ActionResult<object>> GetAggegrateResultsUsingAysncAwait(string ipAddress, List<string> services = null)
        {
            try
            {
                IPConfigDetails ipConfigDetails = new IPConfigDetails();
                //List<IpInfo> IpInfos = new List<IpInfo>();

                //Validate if IPAddress is in correct format
                if (!_multiSvcIntg.ValidateIPAddress(ipAddress))
                {
                    _logger.LogError("IP Address Not Provided");
                    return BadRequest("IP Address Not Provided");
                }

                var defaultServicesLists = new List<string> { ServiceTypes.RDAPService, ServiceTypes.GeoService };
                //Assign Default Values if no services provided
                if (services == null || !services.Any())
                {
                    _logger.LogInformation("Adding default GEOIP and RDAP services to the list");
                    services.AddRange(defaultServicesLists);
                }

                //Filter out the services which are not in the default service lists
                services = services.Where(s => !string.IsNullOrEmpty(s) && defaultServicesLists.Contains(s.ToUpper())).ToList();

                var IpInfos = await _multiSvcIntg.CallApiUsingAsyncAwait(ipAddress, services, _httpClient);

                ipConfigDetails.IpInfos = IpInfos;
                return Ok(ipConfigDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Call services using Task parallel Library.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAggegrateResultsUsingTPL")]
        public async Task<ActionResult<object>> GetAggegrateResultsUsingTPL(string ipAddress, List<string> services = null)
        {
            try
            {
                IPConfigDetails ipConfigDetails = new IPConfigDetails();
                List<IpInfo> IpInfos = new List<IpInfo>();

                //Validate if IPAddress is in correct format
                if (!_multiSvcIntg.ValidateIPAddress(ipAddress))
                {
                    _logger.LogError("IP Address Not Provided");
                    return BadRequest("IP Address Not Provided");
                }

                var defaultServicesLists = new List<string> { ServiceTypes.RDAPService, ServiceTypes.GeoService };
                //Assign Default Values if no services provided
                if (services == null || !services.Any())
                {
                    _logger.LogInformation("Adding default GEOIP and RDAP services to the list");
                    services.AddRange(defaultServicesLists);
                }

                //Filter out the services which are not in the default service lists
                services = services.Where(s => !string.IsNullOrEmpty(s) && defaultServicesLists.Contains(s.ToUpper())).ToList();

                _logger.LogInformation("Call services parallely using Task and wait till all the tasks completed");
                IpInfos = (await Task.WhenAll(services.Select(s => _multiSvcIntg.CallAPIUsingTPI(s.ToUpper(), ipAddress, _httpClient)))).ToList();

                ipConfigDetails.IpInfos = IpInfos;
                return Ok(ipConfigDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
