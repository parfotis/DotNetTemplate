using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Assignment.Models;
using Assignment.Services;

namespace Assignment.Controllers
{
    [ApiController]
    [Route("api/ip")]
    public class IpLookupController : ControllerBase
    {
        private readonly IIpLookupService _ipLookupService;

        public IpLookupController(IIpLookupServiceFactory ipLookupServiceFactory)
        {
            _ipLookupService = ipLookupServiceFactory.CreateIpLookupService(IpLookupServiceType.Database);
        }

        // GET: api/ip/{ip}
        [HttpGet("{ip}")]
        public async Task<ActionResult<IpAddress>> GetIpAddressByIp(string ip)
        {
            if (!IsValidIpAddress(ip))
            {
                return BadRequest("Invalid IP address format.");
            }

            var lookupResult = await _ipLookupService.LookupIp(ip);

            if (lookupResult == null)
            {
                return NotFound($"Information could not be found for IP Address {ip}.");
            }

            return Ok(lookupResult);
        }

        private bool IsValidIpAddress(string ip)
        {
            string ipv4Pattern = @"^(?!0{2,})([1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5]|0)\." +
                                 @"([1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5]|0)\." +
                                 @"([1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5]|0)\." +
                                 @"([1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5]|0)$";

            return Regex.IsMatch(ip, ipv4Pattern);
        }
    }
}