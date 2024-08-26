using Microsoft.AspNetCore.Mvc;
using portalBalance.Data;
using portalBalance.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace portalBalance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly PortalBalanceContext _context;

        public NotificationController(PortalBalanceContext context)
        {
            _context = context;
        }

        [HttpPost("notify")]
        public async Task<IActionResult> LogNotification([FromBody] NotificationLog notificationLog)
        {
            if (notificationLog == null)
            {
                return BadRequest("Invalid notification log data.");
            }


            _context.NotificationLogs.Add(notificationLog);
            await _context.SaveChangesAsync();

            return Ok("Notification logged successfully.");
        }

        [HttpGet("notifications")]
        public async Task<ActionResult<IEnumerable<NotificationLog>>> GetAllNotifications()
        {
            var notifications = await _context.NotificationLogs.ToListAsync();
            return Ok(notifications);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notificationLog = await _context.NotificationLogs.FindAsync(id);
            if (notificationLog == null)
            {
                return NotFound("Notification not found.");
            }

            _context.NotificationLogs.Remove(notificationLog);
            await _context.SaveChangesAsync();

            return Ok("Notification deleted successfully.");
        }
    }
}
