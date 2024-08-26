using portalBalance.Data;
using portalBalance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using portalBalance.Models.DTO;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace portalBalance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrgAdminController : ControllerBase
    {
        private readonly PortalBalanceContext _context;
        private readonly PasswordHasher<OrgAdmin> _passwordHasher;

        public OrgAdminController(PortalBalanceContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<OrgAdmin>();
        }

      
       
        [HttpGet]
        public async Task<IActionResult> GetOrgAdmins()
        {
            var orgAdmins = await _context.OrgAdmins.ToListAsync();
            if (orgAdmins == null)
            {
                return NotFound("this org admin not found");
            }
            return Ok(orgAdmins);
        }

    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrgAdminById(int id)
        {
            var orgAdmin = await _context.OrgAdmins.FindAsync(id);

            if (orgAdmin == null)
            {
                return NotFound("this org admin not found");
            }

            return Ok(orgAdmin);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrgAdmin(int id, [FromBody] OrgAdminDTO model)
        {
            var orgAdmin = await _context.OrgAdmins.FindAsync(id);

            if (orgAdmin == null)
            {
                return NotFound("this org admin not found");
            }

            // Check if the updated email already exists
            if (_context.OrgAdmins.Any(oa => oa.Id != id && oa.Email == model.Email))
            {
                return BadRequest("This email is already in use.");
            }

            orgAdmin.Username = model.Username;
            orgAdmin.NationalID = model.NationalID;
            orgAdmin.Phonenumber = model.Phonenumber;
            orgAdmin.BusinessID = model.BusinessID;

           
            if (orgAdmin.Email != model.Email)
            {
                orgAdmin.Email = model.Email;
            }

          
            if (!string.IsNullOrEmpty(model.Password))
            {
                orgAdmin.Password = _passwordHasher.HashPassword(null, model.Password);
            }

            _context.OrgAdmins.Update(orgAdmin);
            await _context.SaveChangesAsync();

            return Ok(orgAdmin);
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrgAdmin(int id)
        {
            var orgAdmin = await _context.OrgAdmins.FindAsync(id);

            if (orgAdmin == null)
            {
                return NotFound("this org admin not found");
            }

            _context.OrgAdmins.Remove(orgAdmin);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }
    }
}
