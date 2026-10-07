using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RealEstateCRM.API.Services;
using RealEstateCRM.Core.DTOs;
using RealEstateCRM.Core.Entities;
using RealEstateCRM.Core.Enums;
using RealEstateCRM.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateCRM.API.Controllers
{
    /// <summary>
    /// API Controller for managing property listings, creation, image uploads, and retrieval.
    /// Secured with [Authorize] attribute to restrict access to authenticated agents/users only.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("FixedPolicy")]
    public class PropertiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor injecting Unit of Work, Memory Cache, and AutoMapper services.
        /// </summary>
        public PropertiesController(IUnitOfWork unitOfWork, IMemoryCache cache, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves paginated properties with caching support for optimized performance.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetProperties([FromQuery] PropertySpecParams specParams)
        {
            // Define a unique cache key based on pagination and filtering parameters
            string cacheKey = $"properties_{specParams.PageIndex}_{specParams.PageSize}_{specParams.Search}_{specParams.Sort}";

            // Try to retrieve data from memory cache
            if (!_cache.TryGetValue(cacheKey, out PagedResultDto<PropertyDto>? cachedProperties))
            {
                var query = _unitOfWork.Repository<Property>().AsQueryable();

                // Apply search filter on title or address
                if (!string.IsNullOrEmpty(specParams.Search))
                {
                    query = query.Where(p => p.Title.Contains(specParams.Search) || p.Address.Contains(specParams.Search));
                }

                var totalItems = await query.CountAsync();

                // Retrieve paginated records from database
                var properties = await query
                    .Skip((specParams.PageIndex - 1) * specParams.PageSize)
                    .Take(specParams.PageSize)
                    .ToListAsync();

                // Map entities list to DTO list using AutoMapper
                var itemsDto = _mapper.Map<IReadOnlyList<PropertyDto>>(properties);

                cachedProperties = new PagedResultDto<PropertyDto>(specParams.PageIndex, specParams.PageSize, totalItems, itemsDto);

                // Configure cache duration settings
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                // Save formatted result into memory cache
                _cache.Set(cacheKey, cachedProperties, cacheEntryOptions);
            }

            return Ok(cachedProperties);
        }

        /// <summary>
        /// Retrieves a single property record by its primary key ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPropertyById(int id)
        {
            var property = await _unitOfWork.Repository<Property>().GetByIdAsync(id);

            if (property == null)
            {
                return NotFound(new { message = $"Property with ID {id} was not found." });
            }

            // Map domain Entity model to PropertyDto using AutoMapper
            var propertyDto = _mapper.Map<PropertyDto>(property);

            return Ok(propertyDto);
        }

        /// <summary>
        /// Creates a new property listing entry in the system.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map request DTO to Property Entity using AutoMapper
            var property = _mapper.Map<Property>(dto);
            property.Status = PropertyStatus.Available;
            property.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Property>().AddAsync(property);
            await _unitOfWork.CompleteAsync();

            var resultDto = _mapper.Map<PropertyDto>(property);

            return CreatedAtAction(nameof(GetPropertyById), new { id = property.Id }, resultDto);
        }

        /// <summary>
        /// Uploads an image for a specific property listing.
        /// </summary>
        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadPropertyImage(int id, IFormFile file)
        {
            var property = await _unitOfWork.Repository<Property>().GetByIdAsync(id);

            if (property == null)
            {
                return NotFound(new { message = $"Property with ID {id} was not found." });
            }

            var documentService = new DocumentService();
            var imageUrl = await documentService.UploadImageAsync(file);

            if (string.IsNullOrEmpty(imageUrl))
            {
                return BadRequest(new { message = "Please select a valid image file." });
            }

            // Update property image URL path in database
            property.ImageUrl = imageUrl;
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Image uploaded successfully.", imageUrl = imageUrl });
        }
        /// <summary>
        /// Exports all active property listings to a CSV file.
        /// </summary>
        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportPropertiesToCsv()
        {
            var properties = await _unitOfWork.Repository<Property>().GetAllAsync();
            var propertiesDto = _mapper.Map<IReadOnlyList<PropertyDto>>(properties);

            var exportService = new Services.CsvExportService();
            var fileBytes = exportService.ExportToCsv(propertiesDto);

            return File(fileBytes, "text/csv", $"properties_export_{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
        }
    }
}