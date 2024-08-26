using portalBalance.Data;
using portalBalance.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace portalBalance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PortalBalanceContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<SuperAdmin> _superAdminHasher;
        private readonly PasswordHasher<OrgAdmin> _orgAdminHasher;

        public AuthController(PortalBalanceContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _superAdminHasher = new PasswordHasher<SuperAdmin>();
            _orgAdminHasher = new PasswordHasher<OrgAdmin>();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            
            var superAdmin = _context.SuperAdmins.SingleOrDefault(x => x.Email == loginModel.Email);
            if (superAdmin != null && _superAdminHasher.VerifyHashedPassword(superAdmin, superAdmin.Password, loginModel.Password) == PasswordVerificationResult.Success)
            {
                // Generate token for SuperAdmin
                var superAdminToken = GenerateJwtToken(superAdmin.Email, "SuperAdmin", superAdmin.Id, true);
                return Ok(new { Token = superAdminToken });
            }

           
            var orgAdmin = _context.OrgAdmins.SingleOrDefault(x => x.Email == loginModel.Email);
            if (orgAdmin != null && _orgAdminHasher.VerifyHashedPassword(orgAdmin, orgAdmin.Password, loginModel.Password) == PasswordVerificationResult.Success)
            {
                // Generate token for OrgAdmin
                var token = GenerateJwtToken(orgAdmin.Email, "OrgAdmin", orgAdmin.Id, false);
                return Ok(new { Token = token });
            }

            return NotFound("Invalid email or password");
        }

        private string GenerateJwtToken(string email, string role, int userId, bool isSuperAdmin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim("Role", role),
                new Claim("Email", email),
                new Claim("SuperAdminOrOrgAdminId", userId.ToString())
            };

            if (isSuperAdmin)
            {
                claims.Add(new Claim("SuperAdminId", userId.ToString()));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
