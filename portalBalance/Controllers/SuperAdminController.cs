using portalBalance.Data;
using portalBalance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using portalBalance.Models.DTO;
using System.Security.Claims;

namespace portalBalance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly PortalBalanceContext _context;
        private readonly PasswordHasher<OrgAdmin> _passwordHasher;

        public SuperAdminController(PortalBalanceContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<OrgAdmin>();
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost("create-orgadmin")]
        public async Task<IActionResult> CreateOrgAdmin([FromBody] OrgAdminDTO model)
        {
            if (_context.OrgAdmins.Any(oa => oa.Email == model.Email))
            {
                return BadRequest("This email is already in use.");
            }

            // Verify that the organization exists
            var organization = await _context.Organizations.FindAsync(model.OrganizationId);
            if (organization == null)
            {
                return NotFound("Organization not found.");
            }

            var orgAdmin = new OrgAdmin
            {
                Username = model.Username,
                NationalID = model.NationalID,
                Phonenumber = model.Phonenumber,
                BusinessID = model.BusinessID,
                Email = model.Email,
                Password = _passwordHasher.HashPassword(null, model.Password),
                OrganizationId = model.OrganizationId,
                org_Name = organization.Org_Name 
                // This should be the organization name
            };

            _context.OrgAdmins.Add(orgAdmin);
            await _context.SaveChangesAsync();

            return Ok(orgAdmin);
        }
    }
}
