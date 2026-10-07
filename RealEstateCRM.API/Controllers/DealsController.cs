using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateCRM.Core.DTOs;
using RealEstateCRM.Core.Entities;
using RealEstateCRM.Core.Enums;
using RealEstateCRM.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace RealEstateCRM.API.Controllers
{
    /// <summary>
    /// API Controller for managing property sales and deals.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DealsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DealsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new deal and closes property availability.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateDeal([FromBody] CreateDealDto dto)
        {
            var property = await _unitOfWork.Repository<Property>().GetByIdAsync(dto.PropertyId);
            if (property == null)
                return NotFound(new { message = $"Property with ID {dto.PropertyId} not found." });

            var deal = _mapper.Map<Deal>(dto);
            deal.Status = DealStatus.ClosedWon;
            deal.ContractDate = DateTime.UtcNow;

            // Automatically mark property as sold/unavailable upon closing deal
            property.Status = PropertyStatus.Sold;

            await _unitOfWork.Repository<Deal>().AddAsync(deal);
            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map<DealDto>(deal);
            return Ok(result);
        }
    }
}