using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RDAP.API.Service.Models;
using RDAP.API.Service.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace RDAP.API.Service.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class RDAPAPIController : ControllerBase
    {
        private readonly IRdapApiService _rdapApiService;
        private readonly ILogger<RDAPAPIController> _logger;

        public RDAPAPIController(IRdapApiService rdapApiService, ILogger<RDAPAPIController> logger)
        {
            _rdapApiService = rdapApiService;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/v1/RdapInfo/GetRdapInfo")]
        public async Task<ActionResult<IpInfo>> GetRdapInfo(string ipAddress)
        {
            try
            {
                var response = await _rdapApiService.GetRdapInfoByIpAddress(ipAddress);
                _logger.LogInformation($"Response {response}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while calling the GetRdapInfo api {ex?.Message}");
                return BadRequest(ex?.Message);
            }
        }
    }
}
