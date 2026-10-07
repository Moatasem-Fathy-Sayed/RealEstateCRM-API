using System;
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
    public class InteractionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public InteractionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrieves all interactions recorded for a specific lead.
        /// </summary>
        [HttpGet("lead/{leadId}")]
        public async Task<IActionResult> GetInteractionsByLead(int leadId)
        {
            var interactions = await _unitOfWork.Repository<InteractionLog>()
                .AsQueryable()
                .Where(i => i.LeadId == leadId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            var result = interactions.Select(i => new InteractionLogDto
            {
                Id = i.Id,
                LeadId = i.LeadId,
                Type = i.Type.ToString(),
                Notes = i.Notes,
                InteractionDate = i.CreatedAt
            });

            return Ok(result);
        }

        /// <summary>
        /// Logs a new interaction with a lead.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateInteraction([FromBody] CreateInteractionLogDto dto)
        {
            // Verify if the lead exists
            var leadExists = await _unitOfWork.Repository<Lead>()
                .AsQueryable()
                .AnyAsync(l => l.Id == dto.LeadId);

            if (!leadExists)
                return NotFound(new { message = $"Lead with ID {dto.LeadId} was not found." });

            var log = new InteractionLog
            {
                LeadId = dto.LeadId,
                Type = dto.Type,
                Notes = dto.Notes,
                CreatedAt = dto.InteractionDate ?? DateTime.UtcNow
            };

            await _unitOfWork.Repository<InteractionLog>().AddAsync(log);
            await _unitOfWork.CompleteAsync();

            var result = new InteractionLogDto
            {
                Id = log.Id,
                LeadId = log.LeadId,
                Type = log.Type.ToString(),
                Notes = log.Notes,
                InteractionDate = log.CreatedAt
            };

            return Ok(result);
        }
    }
}