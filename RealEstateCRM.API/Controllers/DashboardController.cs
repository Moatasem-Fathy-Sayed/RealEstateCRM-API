using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateCRM.Core.DTOs;
using RealEstateCRM.Core.Entities;
using RealEstateCRM.Core.Interfaces;

namespace RealEstateCRM.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Calculates aggregated executive analytics and CRM key performance indicators (KPIs).
        /// </summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var propertiesCount = await _unitOfWork.Repository<Property>().AsQueryable().CountAsync();
            var leadsQuery = _unitOfWork.Repository<Lead>().AsQueryable();

            var totalLeads = await leadsQuery.CountAsync();
            var totalPipelineBudget = await leadsQuery.SumAsync(l => l.EstimatedBudget);

            var leadsByStatus = await leadsQuery
                .GroupBy(l => l.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(k => k.Status, v => v.Count);

            var totalInteractions = await _unitOfWork.Repository<InteractionLog>().AsQueryable().CountAsync();

            var recentInteractions = await _unitOfWork.Repository<InteractionLog>()
                .AsQueryable()
                .OrderByDescending(i => i.CreatedAt)
                .Take(5)
                .Select(i => new RecentInteractionDto
                {
                    Id = i.Id,
                    Type = i.Type.ToString(),
                    Notes = i.Notes,
                    CreatedAt = i.CreatedAt
                })
                .ToListAsync();

            var summary = new DashboardSummaryDto
            {
                TotalProperties = propertiesCount,
                TotalLeads = totalLeads,
                TotalInteractions = totalInteractions,
                TotalPipelineBudget = totalPipelineBudget,
                LeadsByStatus = leadsByStatus,
                RecentInteractions = recentInteractions
            };

            return Ok(summary);
        }
    }
}