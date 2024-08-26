using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portalBalance.Data;
using portalBalance.Models;
using portalBalance.Models.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace portalBalance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CourseTransactionController : ControllerBase
    {
        private readonly PortalBalanceContext _context;

        public CourseTransactionController(PortalBalanceContext context)
        {
            _context = context;
        }

        [HttpPost("coursetransactions")]
        public async Task<IActionResult> CreateCourseTransaction([FromBody] CourseTransactionDTO model)
        {
            var course = await _context.Courses.FindAsync(model.CourseId);

            if (course == null)
            {
                return BadRequest("Course does not exist.");
            }

            var courseTransaction = new CourseTransaction
            {
                CourseId = model.CourseId,
                CourseName = course.CourseName,
                Date = model.Date,
                TransactionCount = model.TransactionCount,
                TransactionAmount = model.TransactionAmount,
                BalanceBefore = model.BalanceBefore,
                BalanceAfter = model.BalanceAfter,
                UniversityName = model.UniversityName,
                FacultyName = model.FacultyName,
                DepartmentName = model.DepartmentName,
                AcademicYear = model.AcademicYear
            };

            _context.CourseTransactions.Add(courseTransaction);
            await _context.SaveChangesAsync();

            return Ok(courseTransaction);
        }

        [HttpGet("coursetransactions/{id}")]
        public async Task<IActionResult> GetCourseTransaction(int id)
        {
            var courseTransaction = await _context.CourseTransactions.FindAsync(id);

            if (courseTransaction == null)
            {
                return NotFound();
            }

            return Ok(courseTransaction);
        }

        [HttpGet("coursetransactions")]
        public async Task<IActionResult> GetAllCourseTransactions()
        {
            var courseTransactions = await _context.CourseTransactions.ToListAsync();
            return Ok(courseTransactions);
        }

        [HttpPut("coursetransactions/{id}")]
        public async Task<IActionResult> UpdateCourseTransaction(int id, [FromBody] CourseTransactionDTO model)
        {
            var courseTransaction = await _context.CourseTransactions.FindAsync(id);

            if (courseTransaction == null)
            {
                return NotFound();
            }

            courseTransaction.CourseId = model.CourseId;
            courseTransaction.Date = model.Date;
            courseTransaction.TransactionCount = model.TransactionCount;
            courseTransaction.TransactionAmount = model.TransactionAmount;
            courseTransaction.BalanceBefore = model.BalanceBefore;
            courseTransaction.BalanceAfter = model.BalanceAfter;
            courseTransaction.UniversityName = model.UniversityName;
            courseTransaction.FacultyName = model.FacultyName;
            courseTransaction.DepartmentName = model.DepartmentName;
            courseTransaction.AcademicYear = model.AcademicYear;

            _context.CourseTransactions.Update(courseTransaction);
            await _context.SaveChangesAsync();

            return Ok(courseTransaction);
        }

        [HttpDelete("coursetransactions/{id}")]
        public async Task<IActionResult> DeleteCourseTransaction(int id)
        {
            var courseTransaction = await _context.CourseTransactions.FindAsync(id);

            if (courseTransaction == null)
            {
                return NotFound();
            }

            _context.CourseTransactions.Remove(courseTransaction);
            await _context.SaveChangesAsync();

            return Ok(courseTransaction);
        }
    }
}
