using Geo.API.Service.Services.Interfaces;
using GeoIP.API.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace RDAP.API.Service.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class GEOAPIController : ControllerBase
    {
        private readonly IGeoIPApiService _geoIPApiService;
        private readonly ILogger<GEOAPIController> _logger;

        public GEOAPIController(IGeoIPApiService geoIPApiService, ILogger<GEOAPIController> logger)
        {
            _geoIPApiService = geoIPApiService;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/v1/GeoInfo/GetGeoInfo")]
        public async Task<ActionResult<IpInfo>> GetGeoInfo(string ipAddress)
        {
            try
            {
                var response = await _geoIPApiService.GetGeoInfoByIpAddress(ipAddress);
                _logger.LogInformation($"Response {response}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while calling the GetRdapInfo api {ex?.Message}");
                return BadRequest(ex?.Message);
            }
        }
    }
}
