using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstateCRM.Core.DTOs;
using RealEstateCRM.Core.Entities;
using RealEstateCRM.Core.Interfaces;

namespace RealEstateCRM.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("FixedPolicy")]
public class LeadsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LeadsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLeads()
    {
        var leads = await _unitOfWork.Repository<Lead>().GetAllAsync();
        var leadDtos = leads.Select(l => new LeadDto
        {
            Id = l.Id,
            FullName = l.FullName,
            Email = l.Email,
            PhoneNumber = l.PhoneNumber,
            PreferredLocation = l.PreferredLocation,
            EstimatedBudget = l.EstimatedBudget,
            Status = l.Status,
            CreatedAt = l.CreatedAt
        });

        return Ok(leadDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLeadById(int id)
    {
        var lead = await _unitOfWork.Repository<Lead>().GetByIdAsync(id);
        if (lead == null)
            return NotFound(new { Message = "Lead not found." });

        var leadDto = new LeadDto
        {
            Id = lead.Id,
            FullName = lead.FullName,
            Email = lead.Email,
            PhoneNumber = lead.PhoneNumber,
            PreferredLocation = lead.PreferredLocation,
            EstimatedBudget = lead.EstimatedBudget,
            Status = lead.Status,
            CreatedAt = lead.CreatedAt
        };

        return Ok(leadDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLead([FromBody] CreateLeadDto dto)
    {
        var lead = new Lead
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PreferredLocation = dto.PreferredLocation,
            EstimatedBudget = dto.EstimatedBudget
        };

        await _unitOfWork.Repository<Lead>().AddAsync(lead);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetLeadById), new { id = lead.Id }, dto);
    }
    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportLeadsToCsv()
    {
        var leads = await _unitOfWork.Repository<Lead>().GetAllAsync();
        var leadsDto = _mapper.Map<IReadOnlyList<LeadDto>>(leads);

        var exportService = new Services.CsvExportService();
        var fileBytes = exportService.ExportToCsv(leadsDto);

        return File(fileBytes, "text/csv", $"leads_export_{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
    }
}