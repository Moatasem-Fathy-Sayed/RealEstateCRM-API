using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateCRM.Core.DTOs;
using RealEstateCRM.Core.Entities;
using RealEstateCRM.Core.Enums;
using RealEstateCRM.Core.Interfaces;
using System.Threading.Tasks;

namespace RealEstateCRM.API.Controllers
{
    /// <summary>
    /// API Controller for scheduling and managing property viewing appointments.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Schedules a new property viewing appointment.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto dto)
        {
            var appointment = _mapper.Map<Appointment>(dto);
            appointment.Status = AppointmentStatus.Scheduled;

            await _unitOfWork.Repository<Appointment>().AddAsync(appointment);
            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map<AppointmentDto>(appointment);
            return Ok(result);
        }
    }
} 
