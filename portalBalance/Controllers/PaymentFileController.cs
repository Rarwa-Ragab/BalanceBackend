//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using portalBalance.Data;
//using portalBalance.Models;
//using portalBalance.Models.DTO;
//using System;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using System.IO;

//namespace portalBalance.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]

//    public class PaymentFileController : ControllerBase
//    {
//        private readonly PortalBalanceContext _context;

//        public PaymentFileController(PortalBalanceContext context)
//        {
//            _context = context;
//        }

//        [HttpPost("paymentfiles")]
//        public async Task<IActionResult> CreatePaymentFile([FromForm] PaymentFileDTO model)
//        {
//            var orgAdminExists = await _context.OrgAdmins
//                .AnyAsync(o => o.Username == model.UploaderName);

//            var superAdminExists = await _context.SuperAdmins
//                .AnyAsync(s => s.Name == model.UploaderName);

//            if (!orgAdminExists && !superAdminExists)
//            {
//                return BadRequest("The provided uploader name does not match any admin records.");
//            }

//            var organizationExists = await _context.Organizations
//                .AnyAsync(o => o.Org_Name == model.OrganizationName);

//            if (!organizationExists)
//            {
//                return BadRequest("The provided organization name does not exist.");
//            }

//            var paymentFile = new PaymentFile
//            {
//                File = await GetFileBytes(model.File),
//                Organization = model.OrganizationName,
//                UploaderName = model.UploaderName,
//                Date = DateTime.UtcNow.Date,
//                Time = DateTime.UtcNow.TimeOfDay,
//                TotalIncome = model.TotalIncome,
//                Status = model.Status
//            };

//            _context.PaymentFiles.Add(paymentFile);
//            await _context.SaveChangesAsync();

//            return Ok(paymentFile);
//        }
//        [HttpGet("paymentfiles/{id}")]
//        public async Task<IActionResult> GetPaymentFile(int id)
//        {
//            var paymentFile = await _context.PaymentFiles.FindAsync(id);

//            if (paymentFile == null)
//            {
//                return NotFound();
//            }

//            return Ok(paymentFile);
//        }
//        [HttpPut("paymentfiles/{id}")]
//        public async Task<IActionResult> UpdatePaymentFile(int id, [FromForm] PaymentFileDTO model)
//        {
//            var orgAdminExists = await _context.OrgAdmins
//                .AnyAsync(o => o.Username == model.UploaderName);

//            var superAdminExists = await _context.SuperAdmins
//                .AnyAsync(s => s.Name == model.UploaderName);

//            if (!orgAdminExists && !superAdminExists)
//            {
//                return BadRequest("The provided uploader name does not match any admin records.");
//            }

//            var organizationExists = await _context.Organizations
//                .AnyAsync(o => o.Org_Name == model.OrganizationName);

//            if (!organizationExists)
//            {
//                return BadRequest("The provided organization name does not exist.");
//            }

//            var paymentFile = await _context.PaymentFiles.FindAsync(id);

//            if (paymentFile == null)
//            {
//                return NotFound();
//            }

//            paymentFile.File = model.File != null ? await GetFileBytes(model.File) : paymentFile.File;
//            paymentFile.Organization = model.OrganizationName;
//            paymentFile.UploaderName = model.UploaderName;
//            paymentFile.Date = DateTime.UtcNow.Date;
//            paymentFile.Time = DateTime.UtcNow.TimeOfDay;
//            paymentFile.TotalIncome = model.TotalIncome;
//            paymentFile.Status = model.Status;

//            _context.PaymentFiles.Update(paymentFile);
//            await _context.SaveChangesAsync();

//            return Ok(paymentFile);
//        }

//        [HttpDelete("paymentfiles/{id}")]
//        public async Task<IActionResult> DeletePaymentFile(int id)
//        {
//            var paymentFile = await _context.PaymentFiles.FindAsync(id);

//            if (paymentFile == null)
//            {
//                return NotFound();
//            }

//            _context.PaymentFiles.Remove(paymentFile);
//            await _context.SaveChangesAsync();

//            return Ok(paymentFile);
//        }

//        [HttpGet("paymentfiles")]
//        public async Task<IActionResult> GetAllPaymentFiles()
//        {
//            var paymentFiles = await _context.PaymentFiles.ToListAsync();
//            return Ok(paymentFiles);
//        }

//        private async Task<byte[]> GetFileBytes(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                return null;
//            }

//            using (var memoryStream = new MemoryStream())
//            {
//                await file.CopyToAsync(memoryStream);
//                return memoryStream.ToArray();
//            }
//        }
//    }
//}
