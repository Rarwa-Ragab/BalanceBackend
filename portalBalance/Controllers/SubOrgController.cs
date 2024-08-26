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

    public class SubOrgController : ControllerBase
    {
        private readonly PortalBalanceContext _context;

        public SubOrgController(PortalBalanceContext context)
        {
            _context = context;
        }

        [HttpPost("suborgs")]
        public async Task<IActionResult> CreateSubOrg([FromBody] SubOrgDTO model)
        {
            if (!await _context.Organizations.AnyAsync(o => o.Id == model.OrganizationId))
            {
                return BadRequest("Organization does not exist.");
            }

           
            var subOrg = new SubOrg
            {
                ParentOrg_Name = model.ParentOrg_Name,
                SubOrg_Name = model.SubOrg_Name,
                OrgType = model.OrgType,
                BankAcount = model.BankAcount,
                License_ID = model.License_ID,
                Momkn_ID = model.Momkn_ID,
                Momkn_Financial_ID = model.Momkn_Financial_ID,
                State = model.State,
               
                OrganizationId = model.OrganizationId
            };

            _context.SubOrgs.Add(subOrg);
            await _context.SaveChangesAsync();

            return Ok(subOrg);
        }

        [HttpGet("suborgs/{id}")]
        public async Task<IActionResult> GetSubOrg(int id)
        {
            var subOrg = await _context.SubOrgs.FindAsync(id);

            if (subOrg == null)
            {
                return NotFound();
            }

            return Ok(subOrg);
        }

        [HttpPut("suborgs/{id}")]
        public async Task<IActionResult> UpdateSubOrg(int id, [FromBody] SubOrgDTO model)
        {
            if (!await _context.Organizations.AnyAsync(o => o.Id == model.OrganizationId))
            {
                return BadRequest("Organization does not exist.");
            }

            var subOrg = await _context.SubOrgs.FindAsync(id);

            if (subOrg == null)
            {
                return NotFound();
            }

            subOrg.ParentOrg_Name = model.ParentOrg_Name;
            subOrg.SubOrg_Name = model.SubOrg_Name;
            subOrg.OrgType = model.OrgType;
            subOrg.BankAcount = model.BankAcount;
            subOrg.License_ID = model.License_ID;
            subOrg.Momkn_ID = model.Momkn_ID;
            subOrg.Momkn_Financial_ID = model.Momkn_Financial_ID;
            subOrg.State = model.State;
            subOrg.OrganizationId = model.OrganizationId;

            _context.SubOrgs.Update(subOrg);
            await _context.SaveChangesAsync();

            return Ok(subOrg);
        }

        [HttpDelete("suborgs/{id}")]
        public async Task<IActionResult> DeleteSubOrg(int id)
        {
            var subOrg = await _context.SubOrgs.FindAsync(id);

            if (subOrg == null)
            {
                return NotFound();
            }

            _context.SubOrgs.Remove(subOrg);
            await _context.SaveChangesAsync();

            return Ok(subOrg);
        }

        [HttpGet("suborgs")]
        public async Task<IActionResult> GetAllSubOrgs()
        {
            var subOrgs = await _context.SubOrgs.ToListAsync();
            return Ok(subOrgs);
        }
        [HttpGet("suborg_by_orgid/{organizationId}")]
        public async Task<IActionResult> GetSubOrgsByOrgId(int organizationId)
        {
            var subOrgs = await _context.SubOrgs
                .Where(s => s.OrganizationId == organizationId)
                .ToListAsync();

            if (subOrgs == null || !subOrgs.Any())
            {
                return NotFound();
            }

            return Ok(subOrgs);
        }
    }
}
