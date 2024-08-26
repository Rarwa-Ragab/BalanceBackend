using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using portalBalance.Data;
using portalBalance.Models;
using portalBalance.Models.DTO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace portalBalance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorTransactionController : ControllerBase
    {
        private readonly PortalBalanceContext _context;

        public ProfessorTransactionController(PortalBalanceContext context)
        {
            _context = context;
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> CreateProfessorTransaction([FromBody] ProfessorTransactionDTO model)
        {
            var professor = await _context.Professors
                .FirstOrDefaultAsync(p => p.NationalID == model.NationalId);

            if (professor == null)
            {
                return BadRequest("Professor does not exist.");
            }

      
            if (model.Type == TransactionType.Debit &&
                professor.Professorbalance < model.TransactionAmount)
            {
                return BadRequest(new
                {
                    Message = "Insufficient balance for debit transaction.",
                    AvailableBalance = professor.Professorbalance
                });
            }

            var transaction = new ProfessorTransaction
            {
                NationalId = model.NationalId,
                MobileNumber = professor.PhoneNumber,
                Date = model.Date,
                TransactionReference = model.TransactionReference,
                Reference2 = model.Reference2,
                TransactionAmount = model.TransactionAmount,
                Type = model.Type,
                UniversityName = professor.Org_Name,
                FacultyName = professor.Sub_Org_Name,
                Status = model.Status,
                ProfessorId = professor.Id,
                ProfessorName = professor.Username,
                Professor = professor,
            };

            _context.ProfessorTransaction.Add(transaction);

            // Update professor's balance based on the transaction type and status
            if (model.Status == TransactionStatus.Approved)
            {
                if (model.Type == TransactionType.Credit)
                {
                    professor.Professorbalance += model.TransactionAmount;
                }
                else if (model.Type == TransactionType.Debit)
                {
                    professor.Professorbalance -= model.TransactionAmount;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(transaction);
        }

        [HttpGet("transactions/{id}")]
        public async Task<IActionResult> GetProfessorTransaction(int id)
        {
            var transaction = await _context.ProfessorTransaction
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            var transactionDTO = new ProfessorTransactionDTO
            {
                Id = transaction.Id,
                NationalId = transaction.NationalId,
                MobileNumber = transaction.MobileNumber,
                ProfessorName = transaction.ProfessorName,
                Date = transaction.Date,
                TransactionReference = transaction.TransactionReference,
                Reference2 = transaction.Reference2,
                TransactionAmount = transaction.TransactionAmount,
                Type = transaction.Type,
                UniversityName = transaction.UniversityName,
                FacultyName = transaction.FacultyName,
                Status = transaction.Status,
                ProfessorId = transaction.ProfessorId,
                NotificationSent = transaction.NotificationSent,
            };

            return Ok(transactionDTO);
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetAllProfessorTransactions()
        {
            var transactions = await _context.ProfessorTransaction.ToListAsync();

            var transactionDTOs = transactions.Select(transaction => new ProfessorTransactionDTO
            {
                Id = transaction.Id,
                NationalId = transaction.NationalId,
                MobileNumber = transaction.MobileNumber,
                ProfessorName = transaction.ProfessorName,
                Date = transaction.Date,
                TransactionReference = transaction.TransactionReference,
                Reference2 = transaction.Reference2,
                TransactionAmount = transaction.TransactionAmount,
                Type = transaction.Type,
                UniversityName = transaction.UniversityName,
                FacultyName = transaction.FacultyName,
                Status = transaction.Status,
                ProfessorId = transaction.ProfessorId,
                NotificationSent = transaction.NotificationSent,
            }).ToList();

            return Ok(transactionDTOs);
        }

        [HttpPatch("transactions/{id}/status")]
        public async Task<IActionResult> UpdateTransactionStatus(int id, [FromBody] JsonElement jsonElement)
        {
            if (!jsonElement.TryGetProperty("status", out JsonElement statusElement))
            {
                return BadRequest("Status property is missing.");
            }

            var statusString = statusElement.GetString();
            if (string.IsNullOrEmpty(statusString))
            {
                return BadRequest("Status value is empty.");
            }

            var transaction = await _context.ProfessorTransaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

      
            var newStatus = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), statusString, true);

           
            transaction.Status = newStatus;

            var professor = await _context.Professors.FirstOrDefaultAsync(p => p.NationalID == transaction.NationalId);
            if (professor == null)
            {
                return NotFound("Professor does not exist.");
            }

            // Update professor's balance based on the transaction type and new status
            if (newStatus == TransactionStatus.Approved)
            {
                if (transaction.Type == TransactionType.Credit)
                {
                    professor.Professorbalance += transaction.TransactionAmount;
                }
             
            }

            // Send notification 
            if (newStatus == TransactionStatus.Approved && !transaction.NotificationSent)
            {
                await SendNotificationToExternalApi(transaction);
                transaction.NotificationSent = true;
            }

            _context.ProfessorTransaction.Update(transaction);
            await _context.SaveChangesAsync();

            return Ok("Balance added and Notification send successfully");
        }

        private async Task SendNotificationToExternalApi(ProfessorTransaction transaction)
        {
            try
            {
                using var httpClient = new HttpClient();

                var requestBody = new
                {
                    nationalId = transaction.NationalId,
                    title = $"You have a new credit dear {transaction.ProfessorName}",
                    body = $"{transaction.TransactionAmount} EGP has been deposited" +
                    $" and your current balance is {transaction.Professor.Professorbalance} EGP"
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var apiUrl = "https://ahly-momken-cashout.onrender.com/notification/balanceNotification";

                var response = await httpClient.PostAsync(apiUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Notification sent successfully for transaction ID: {transaction.Id}");
                }
                else
                {
                    Console.WriteLine($"Failed to send notification for transaction ID: {transaction.Id}. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while sending notification for transaction ID {transaction.Id}: {ex.Message}");
            }
        }

        [HttpGet("transactions/{nationalId}/last-balance")]
        public async Task<IActionResult> GetLastAvailableBalanceAfter(string nationalId)
        {
            var lastTransaction = await _context.ProfessorTransaction
                .Where(t => t.NationalId == nationalId)
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync();

            if (lastTransaction == null)
            {
                return NotFound("No transactions found for the specified National ID.");
            }

            var professor = await _context.Professors
                .FirstOrDefaultAsync(p => p.NationalID == nationalId);

            if (professor == null)
            {
                return NotFound("Professor does not exist.");
            }

            return Ok(professor.Professorbalance);
        }

        [HttpDelete("transactions")]
        public async Task<IActionResult> DeleteAllProfessorTransactions()
        {
            var transactions = await _context.ProfessorTransaction.ToListAsync();

            if (transactions.Count == 0)
            {
                return NotFound("No transactions found to delete.");
            }

            _context.ProfessorTransaction.RemoveRange(transactions);
            await _context.SaveChangesAsync();

            return Ok("All transactions have been deleted.");
        }

        [HttpDelete("transactions/{id}")]
        public async Task<IActionResult> DeleteProfessorTransaction(int id)
        {
            var transaction = await _context.ProfessorTransaction.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            _context.ProfessorTransaction.Remove(transaction);
            await _context.SaveChangesAsync();

            return Ok(transaction);
        }
    }
}
