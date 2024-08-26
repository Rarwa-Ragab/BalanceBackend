using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portalBalance.Data;
using portalBalance.Models;
using portalBalance.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace portalBalance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CourseController : ControllerBase
    {
        private readonly PortalBalanceContext _context;

        public CourseController(PortalBalanceContext context)
        {
            _context = context;
        }

        [HttpPost("courses")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseDTO model)
        {
            if (!await _context.Departments.AnyAsync(d => d.Id == model.DepartmentId))
            {
                return BadRequest("Department does not exist.");
            }

            var course = new Course
            {
                CourseName = model.CourseName,
                DepartmentName = model.DepartmentName,
                EducationYear = model.EducationYear,
                EducationTerm = model.EducationTerm,
                CourseHours = model.CourseHours,
                PricePerHour = model.PricePerHour,
                State = model.State,
                DepartmentId = model.DepartmentId,
                CourseBalance = model.CourseBalance,
                LastUpdate = model.LastUpdate,
                universityName = model.universityName,
                FaculityName = model.FaculityName,
            };

            var professorCourses = new List<ProfessorCourse>();
            foreach (var professorCourseDTO in model.Professors)
            {
                var professor = await _context.Professors.FindAsync(professorCourseDTO.ProfessorId);
                if (professor == null)
                {
                    return BadRequest($"Professor with ID {professorCourseDTO.ProfessorId} does not exist.");
                }

                var professorCourse = new ProfessorCourse
                {
                    Course = course,
                    Professor = professor,
                    ProfShare = professorCourseDTO.ProfShare
                };

                professorCourses.Add(professorCourse);
            }

            course.ProfessorCourses = professorCourses;

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var courseDTO = new CourseDTO
            {
                CourseName = course.CourseName,
                DepartmentName = course.DepartmentName,
                EducationYear = course.EducationYear,
                EducationTerm = course.EducationTerm,
                CourseHours = course.CourseHours,
                PricePerHour = course.PricePerHour,
                State = course.State,
                DepartmentId = course.DepartmentId,
                CourseBalance = course.CourseBalance,
                LastUpdate = course.LastUpdate,
                universityName = course.universityName,
                FaculityName = course.FaculityName,
                Professors = course.ProfessorCourses.Select(pc => new ProfessorCourseDTO
                {
                    ProfessorId = pc.ProfessorId,
                    ProfessorName = pc.Professor.Username, 
                    ProfShare = pc.ProfShare
                }).ToList()
            };

            return Ok(courseDTO);
        }

        [HttpGet("courses/{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.ProfessorCourses)
                .ThenInclude(pc => pc.Professor)
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var courseDTO = new CourseDTO
            {
                Id = course.Id,
                CourseName = course.CourseName,
                DepartmentName = course.DepartmentName,
                EducationYear = course.EducationYear,
                EducationTerm = course.EducationTerm,
                CourseHours = course.CourseHours,
                PricePerHour = course.PricePerHour,
                State = course.State,
                universityName= course.universityName,
                FaculityName= course.FaculityName,
                DepartmentId = course.DepartmentId,
                CourseBalance = course.CourseBalance,
                LastUpdate = course.LastUpdate,
                Professors = course.ProfessorCourses.Select(pc => new ProfessorCourseDTO
                {
                    ProfessorId = pc.Professor.Id,
                    ProfessorName = pc.Professor.Username,
                    ProfShare = pc.ProfShare
                }).ToList(),
            };

            return Ok(courseDTO);
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.ProfessorCourses)
                .ThenInclude(pc => pc.Professor)
                .Include(c => c.Transactions) 
                .ToListAsync();

            var courseDTOs = courses.Select(course => new CourseDTO
            {
                Id = course.Id,
                CourseName = course.CourseName,
                DepartmentName = course.DepartmentName,
                EducationYear = course.EducationYear,
                EducationTerm = course.EducationTerm,
                CourseHours = course.CourseHours,
                PricePerHour = course.PricePerHour,
                State = course.State,
                universityName = course.universityName,
                FaculityName = course.FaculityName,
                DepartmentId = course.DepartmentId,
                CourseBalance = course.CourseBalance,
                LastUpdate = course.LastUpdate,
                Professors = course.ProfessorCourses.Select(pc => new ProfessorCourseDTO
                {
                    ProfessorId = pc.Professor.Id,
                    ProfessorName = pc.Professor.Username,
                    ProfShare = pc.ProfShare
                }).ToList(),
            }).ToList();

            return Ok(courseDTOs);
        }

        [HttpPut("courses/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDTO model)
        {
            if (!await _context.Departments.AnyAsync(d => d.Id == model.DepartmentId))
            {
                return BadRequest("Department does not exist.");
            }

            var course = await _context.Courses
                .Include(c => c.ProfessorCourses)
                .Include(c => c.Transactions) 
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            course.CourseName = model.CourseName;
            course.DepartmentName = model.DepartmentName;
            course.EducationYear = model.EducationYear;
            course.EducationTerm = model.EducationTerm;
            course.CourseHours = model.CourseHours;
            course.PricePerHour = model.PricePerHour;
            course.State = model.State;
            course.DepartmentId = model.DepartmentId;
            course.CourseBalance = model.CourseBalance;
            course.LastUpdate = model.LastUpdate;

            // Update professor courses
            _context.ProfessorCourses.RemoveRange(course.ProfessorCourses);

            var professorCourses = new List<ProfessorCourse>();
            foreach (var professorCourseDTO in model.Professors)
            {
                var professor = await _context.Professors.FindAsync(professorCourseDTO.ProfessorId);
                if (professor == null)
                {
                    return BadRequest($"Professor with ID {professorCourseDTO.ProfessorId} does not exist.");
                }

                var professorCourse = new ProfessorCourse
                {
                    Course = course,
                    Professor = professor,
                    ProfShare = professorCourseDTO.ProfShare
                };

                professorCourses.Add(professorCourse);
            }

            course.ProfessorCourses = professorCourses;

          

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }
        [HttpPatch("courses/{id}/update-balance")]
        public async Task<IActionResult> UpdateCourseBalance(int id, [FromBody] decimal addBalance)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            course.CourseBalance += addBalance;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        [HttpDelete("courses/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.ProfessorCourses)
                .Include(c => c.Transactions) 
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            _context.ProfessorCourses.RemoveRange(course.ProfessorCourses);
            _context.CourseTransactions.RemoveRange(course.Transactions); 

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        [HttpDelete("courses")]
        public async Task<IActionResult> DeleteAllCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.ProfessorCourses)
                .Include(c => c.Transactions) 
                .ToListAsync();

            if (courses == null || !courses.Any())
            {
                return NotFound();
            }

            foreach (var course in courses)
            {
                _context.ProfessorCourses.RemoveRange(course.ProfessorCourses);
                _context.CourseTransactions.RemoveRange(course.Transactions); 
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();

            return Ok("All courses and related data have been deleted.");
        }

    }
}
