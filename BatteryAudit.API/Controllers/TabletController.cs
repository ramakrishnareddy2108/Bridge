namespace BatteryAudit.API.Controllers
{
    using BatteryAudit.Models;
    using BatteryAudit.Service;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class TabletController : ControllerBase
    {
        private readonly ITabletService _tabletService;
        public TabletController(ITabletService tabletService)
        {
            _tabletService = tabletService;
        }

        /// <summary>
        /// Get average battery usage for a tablet
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>Average battery usage</returns>
        [HttpGet("usage/{serialNumber}")]
        public ActionResult<TabletUsage> GetTabletBatteryUsage([Required]string serialNumber)
        {
            return Ok(_tabletService.GetTabletBatteryUsage(serialNumber));
        }

        /// <summary>
        /// Process battery usage for all tablets available in a acadamy
        /// </summary>
        /// <param name="acadamyId"></param>
        /// <returns>Average battery usage for each tablet</returns>
        [HttpGet("acadamy-usage/{acadamyId}")]
        public ActionResult<IEnumerable<TabletUsage>> GetAcadamyTabletBatteryUsage([Range(1, int.MaxValue, ErrorMessage = "Invalid AcadamyId provided")]int acadamyId) //Assuming acadamy id as an integer
        {
            return Ok(_tabletService.GetAcadamyTabletsBatteryUsage(acadamyId));
        }
    }
}
