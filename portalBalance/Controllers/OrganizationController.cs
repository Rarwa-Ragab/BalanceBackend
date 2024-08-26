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
   
    public class OrganizationController : ControllerBase
    {
        private readonly PortalBalanceContext _context;

        public OrganizationController(PortalBalanceContext context)
        {
            _context = context;
        }

        [HttpPost("organizations")]
        public async Task<IActionResult> CreateOrganization([FromBody] OrganizationDTO model)
        {
            

            var organization = new Organization
            {
                Org_Name = model.Org_Name,
                OrgType = model.OrgType,
                BankAcount = model.BankAcount,
                License_ID = model.License_ID,
                Momkn_ID = model.Momkn_ID,
                Momkn_Financial_ID = model.Momkn_Financial_ID,
                State = model.State,
                UniveristyCut = model.UniveristyCut,
              
            };

            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();

            return Ok(organization);
        }

        [HttpGet("organizations/{id}")]
        public async Task<IActionResult> GetOrganization(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            return Ok(organization);
        }

        [HttpPut("organizations/{id}")]
        public async Task<IActionResult> UpdateOrganization(int id, [FromBody] OrganizationDTO model)
        {
            var organization = await _context.Organizations.FindAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            organization.Org_Name = model.Org_Name;
            organization.OrgType = model.OrgType;
            organization.BankAcount = model.BankAcount;
            organization.License_ID = model.License_ID;
            organization.Momkn_ID = model.Momkn_ID;
            organization.Momkn_Financial_ID = model.Momkn_Financial_ID;
            organization.State = model.State;
            organization.UniveristyCut = model.UniveristyCut;

            _context.Organizations.Update(organization);
            await _context.SaveChangesAsync();

            return Ok(organization);
        }

        [HttpDelete("organizations/{id}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            _context.Organizations.Remove(organization);
            await _context.SaveChangesAsync();

            return Ok(organization);
        }

        [HttpGet("organizations")]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var organizations = await _context.Organizations.ToListAsync();
            return Ok(organizations);
        }
    }
}
