using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateCRM.Core.Entities;
using RealEstateCRM.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateCRM.API.Controllers
{
    /// <summary>
    /// API Controller for managing system notifications and user alerts.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrieves all unread notifications for the system.
        /// </summary>
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            var notifications = await _unitOfWork.Repository<Notification>()
                .AsQueryable()
                .Where(n => !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return Ok(notifications);
        }

        /// <summary>
        /// Marks a specific notification as read.
        /// </summary>
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification = await _unitOfWork.Repository<Notification>().GetByIdAsync(id);

            if (notification == null)
            {
                return NotFound(new { message = $"Notification with ID {id} was not found." });
            }

            notification.IsRead = true;
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Notification marked as read successfully." });
        }
    }
}