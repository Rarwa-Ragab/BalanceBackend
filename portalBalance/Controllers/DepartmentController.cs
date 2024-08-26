using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portalBalance.Data;
using portalBalance.Models;
using portalBalance.Models.DTO;

namespace portalBalance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class DepartmentController : ControllerBase
    {
        private readonly PortalBalanceContext _context;

        public DepartmentController(PortalBalanceContext context)
        {
            _context = context;
        }

        [HttpPost("departments")]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDTO model)
        {
            if (!await _context.SubOrgs.AnyAsync(so => so.Id == model.SubOrgId))
            {
                return BadRequest("SubOrganization does not exist.");
            }

         

            var department = new Department
            {
                Department_Name = model.Department_Name,
                Org_Name = model.Org_Name,
                Sub_Org_Name = model.Sub_Org_Name,
                State = model.State,
               
                SubOrgId = model.SubOrgId
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        [HttpGet("departments/{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpPut("departments/{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDTO model)
        {
            if (!await _context.SubOrgs.AnyAsync(so => so.Id == model.SubOrgId))
            {
                return BadRequest("SubOrganization does not exist.");
            }

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            department.Department_Name = model.Department_Name;
            department.Org_Name = model.Org_Name;
            department.Sub_Org_Name = model.Sub_Org_Name;
            department.State = model.State;
            department.SubOrgId = model.SubOrgId;

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        [HttpDelete("departments/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }
        [HttpDelete("departments")]
        public async Task<IActionResult> DeleteAllDepartments()
        {
            var Departments = await _context.Departments.ToListAsync();

            if (Departments == null || !Departments.Any())
            {
                return NotFound();
            }

            _context.Departments.RemoveRange(Departments);
            await _context.SaveChangesAsync();

            return Ok("All Departments have been deleted.");
        }

        [HttpGet("departments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _context.Departments.ToListAsync();
            return Ok(departments);
        }
        [HttpGet("departments_by_suborgid/{subOrgId}")]
        public async Task<IActionResult> GetDepartmentsBySubOrgId(int subOrgId)
        {
            var departments = await _context.Departments
                .Where(d => d.SubOrgId == subOrgId)
                .ToListAsync();

            if (departments == null || !departments.Any())
            {
                return NotFound();
            }

            return Ok(departments);
        }

    }
}
