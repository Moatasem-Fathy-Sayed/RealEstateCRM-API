using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace RealEstateCRM.API.Controllers
{
    /// <summary>
    /// API Controller for retrieving and updating system-wide configurations.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSystemSettings()
        {
            var settings = new Dictionary<string, object>
            {
                { "SystemName", "RealEstate CRM Enterprise Platform" },
                { "DefaultCurrency", "EGP" },
                { "DefaultCommissionRate", 2.5 },
                { "EnableAutoEmailReminders", true },
                { "SupportEmail", "support@realestatecrm.com" }
            };

            return Ok(settings);
        }
    }
}