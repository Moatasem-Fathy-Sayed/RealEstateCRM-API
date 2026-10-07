using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateCRM.Core.Entities;
using RealEstateCRM.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateCRM.API.Controllers
{
    /// <summary>
    /// API Controller for retrieving system audit logs and change tracking history.
    /// Secured for administrative review.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditLogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrieves the recent system audit logs ordered by creation date.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAuditLogs([FromQuery] int pageSize = 50)
        {
            var logs = await _unitOfWork.Repository<AuditLog>()
                .AsQueryable()
                .OrderByDescending(a => a.CreatedAt)
                .Take(pageSize)
                .ToListAsync();

            return Ok(logs);
        }
    }
}