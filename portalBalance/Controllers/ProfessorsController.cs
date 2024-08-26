using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portalBalance.Data;
using portalBalance.Models;
using portalBalance.Models.DTO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace portalBalance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class ProfessorController : ControllerBase
    {
        private readonly PortalBalanceContext _context;

        public ProfessorController(PortalBalanceContext context)
        {
            _context = context;
        }

        [HttpPost("professors")]
        public async Task<IActionResult> CreateProfessor([FromBody] ProfessorDTO model)
        {
            var existingProfessor = await _context.Professors
                .FirstOrDefaultAsync(p => p.NationalID == model.NationalID);

            if (existingProfessor != null)
            {
                return BadRequest("NationalID already exists.");
            }

            var professor = new Professor
            {
                Username = model.Username,
                NationalID = model.NationalID,
                PhoneNumber = model.PhoneNumber,
                State = model.State,
                Org_Name = model.Org_Name,
                Sub_Org_Name = model.Sub_Org_Name,
                Professorbalance = model.Professorbalance,
            };

            _context.Professors.Add(professor);
            await _context.SaveChangesAsync();

            return Ok(professor);
        }

        [HttpGet("professors")]
        public async Task<IActionResult> GetAllProfessors()
        {
            var professors = await _context.Professors
                .Include(p => p.ProfessorCourses)
                    .ThenInclude(pc => pc.Course)
                .ToListAsync();

            var response = professors.Select(professor => new ProfessorDTO
            {
                Id = professor.Id,
                Username = professor.Username,
                NationalID = professor.NationalID,
                PhoneNumber = professor.PhoneNumber,
                State = professor.State,
                Org_Name = professor.Org_Name,
                Professorbalance= professor.Professorbalance,
                Sub_Org_Name = professor.Sub_Org_Name,
                Courses = professor.ProfessorCourses.Select(pc => new CourseDTO
                {
                    Id = pc.Course.Id,
                    CourseName = pc.Course.CourseName,
                    DepartmentName = pc.Course.DepartmentName,
                    EducationYear = pc.Course.EducationYear,
                    EducationTerm = pc.Course.EducationTerm,
                    CourseHours = pc.Course.CourseHours,
                    PricePerHour = pc.Course.PricePerHour,
                    State = pc.Course.State,
                    CourseBalance = pc.Course.CourseBalance,
                    LastUpdate = pc.Course.LastUpdate
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        [HttpGet("professors/{id}")]
        public async Task<IActionResult> GetProfessorById(int id)
        {
            var professor = await _context.Professors
                .Include(p => p.ProfessorCourses)
                    .ThenInclude(pc => pc.Course)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (professor == null)
            {
                return NotFound();
            }

            var professorDTO = new ProfessorDTO
            {
                Id = professor.Id,
                Username = professor.Username,
                NationalID = professor.NationalID,
                PhoneNumber = professor.PhoneNumber,
                State = professor.State,
                Professorbalance = professor.Professorbalance,
                Org_Name = professor.Org_Name,
                Sub_Org_Name = professor.Sub_Org_Name,
                Courses = professor.ProfessorCourses.Select(pc => new CourseDTO
                {
                    Id = pc.Course.Id,
                    CourseName = pc.Course.CourseName,
                    DepartmentName = pc.Course.DepartmentName,
                    EducationYear = pc.Course.EducationYear,
                    EducationTerm = pc.Course.EducationTerm,
                    CourseHours = pc.Course.CourseHours,
                    PricePerHour = pc.Course.PricePerHour,
                    State = pc.Course.State,
                    CourseBalance = pc.Course.CourseBalance,
                    LastUpdate = pc.Course.LastUpdate
                }).ToList()
            };

            return Ok(professorDTO);
        }

        [HttpPut("professors/{id}")]
        public async Task<IActionResult> UpdateProfessor(int id, [FromBody] ProfessorDTO model)
        {
            var professor = await _context.Professors.FindAsync(id);

            if (professor == null)
            {
                return NotFound();
            }

            var existingProfessor = await _context.Professors
                .FirstOrDefaultAsync(p => p.NationalID == model.NationalID && p.Id != id);

            if (existingProfessor != null)
            {
                return BadRequest("NationalID already exists.");
            }

            professor.Username = model.Username;
            professor.NationalID = model.NationalID;
            professor.PhoneNumber = model.PhoneNumber;
            professor.State = model.State;
          
            professor.Org_Name = model.Org_Name;
            professor.Sub_Org_Name = model.Sub_Org_Name;

            _context.Professors.Update(professor);
            await _context.SaveChangesAsync();

            return Ok(professor);
        }

        [HttpDelete("professors/{id}")]
        public async Task<IActionResult> DeleteProfessor(int id)
        {
            var professor = await _context.Professors.FindAsync(id);

            if (professor == null)
            {
                return NotFound();
            }

            _context.Professors.Remove(professor);
            await _context.SaveChangesAsync();

            return Ok(professor);
        }
        [HttpDelete("professors")]
        public async Task<IActionResult> DeleteAllProfessors()
        {
            var professors = await _context.Professors.ToListAsync();

            if (professors == null || !professors.Any())
            {
                return NotFound();
            }

            _context.Professors.RemoveRange(professors);
            await _context.SaveChangesAsync();

            return Ok("All professors have been deleted.");
        }

    }
}
