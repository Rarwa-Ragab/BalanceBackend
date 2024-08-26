using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portalBalance.Data;
using portalBalance.Models;
using portalBalance.Models.DTO;

namespace portalBalance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CsvUploadController : ControllerBase
    {
        private readonly PortalBalanceContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CsvUploadController> _logger;
        private readonly Random _random;

        public CsvUploadController(PortalBalanceContext context, IHttpClientFactory httpClientFactory, ILogger<CsvUploadController> logger)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _random = new Random();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            List<UniversityExpense> records = new List<UniversityExpense>();

            if (extension == ".csv")
            {
                using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    records = csv.GetRecords<UniversityExpense>().ToList();
                }
            }
            else if (extension == ".xls" || extension == ".xlsx")
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = file.OpenReadStream())
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var dataTable = result.Tables[0];

                    for (int row = 1; row < dataTable.Rows.Count; row++)
                    {
                        var record = new UniversityExpense
                        {
                            BillTypeCode = Convert.ToInt32(dataTable.Rows[row][0]),
                            BillReferenceNo = Convert.ToInt64(dataTable.Rows[row][1]),
                            BillTypeName = dataTable.Rows[row][2].ToString(),
                            TransactionDate = Convert.ToDateTime(dataTable.Rows[row][3]),
                            Time = dataTable.Rows[row][4].ToString(),
                            BankCode = dataTable.Rows[row][5].ToString(),
                            BankName = dataTable.Rows[row][6].ToString(),
                            NationalId = Convert.ToInt64(dataTable.Rows[row][7]),
                            StudentCode = Convert.ToInt64(dataTable.Rows[row][8]),
                            StudentName = dataTable.Rows[row][9].ToString(),
                            PaidAmount = Convert.ToDecimal(dataTable.Rows[row][10]),
                            TuitionFees = Convert.ToDecimal(dataTable.Rows[row][11]),
                            BookFees = Convert.ToDecimal(dataTable.Rows[row][12]),
                            ReconStatus = dataTable.Rows[row][13].ToString(),
                            FacultyCode = dataTable.Rows[row][14].ToString(),
                            FacultyName = dataTable.Rows[row][15].ToString(),
                            AcademicYear = dataTable.Rows[row][16].ToString(),
                            ClassSemester = dataTable.Rows[row][17].ToString(),
                            StudyNature = dataTable.Rows[row][18].ToString(),
                            AsNode = dataTable.Rows[row][19].ToString(),
                            PhaseNode = dataTable.Rows[row][20].ToString(),
                            IsCalculated = false // Default value
                        };

                        records.Add(record);
                    }
                }
            }
            else
            {
                return BadRequest("Unsupported file type.");
            }

            try
            {
                // Insert data into the database
                await _context.UniversityExpenses.AddRangeAsync(records);
                await _context.SaveChangesAsync();

              
                var newRecords = _context.UniversityExpenses.Where(r => !r.IsCalculated).ToList();
                var filteredRecords = newRecords
                    .GroupBy(r => new { r.BillTypeName, r.FacultyName, r.AsNode, r.PhaseNode })
                     .Select(g => new
                     {
                         Key = g.Key,
                         TotalPaidAmount = g.Sum(x => x.PaidAmount),
                         RowCount = g.Count() 
                     }).ToList();


                foreach (var group in filteredRecords)
                {
                    Console.WriteLine($"COUNTTTTTTTTTTTTTTTTTTTTTTT^^^^^^^^^^^^^^^^^^$$$" +
                        $"$$$$$$$$$: {group.Key.BillTypeName}," +
                        $" {group.Key.FacultyName}, {group.Key.AsNode}, " +
                        $"{group.Key.PhaseNode}, " +
                        $"Row Count: {group.RowCount}," +
                        $" total amount:{group.TotalPaidAmount}");


                    var courses = await _context.Courses
                        .Where(c => c.DepartmentName == group.Key.AsNode
                                    && c.FaculityName == group.Key.FacultyName
                                    && c.universityName == group.Key.BillTypeName
                                    && c.EducationYear == group.Key.PhaseNode)
                        .ToListAsync();

                    var totalHours = courses.Sum(c => (decimal)c.CourseHours);

                    foreach (var course in courses)
                    {
                        var courseBalanceAddition = group.TotalPaidAmount * 0.85m * (course.CourseHours / totalHours);
                        course.CourseBalance += courseBalanceAddition;
                      
                        course.LastUpdate = DateTime
                            .UtcNow.AddMilliseconds(_random.Next(1, 1000));

                        if (course.Id == 0)
                        {
                            _logger.LogError($"Course ID is zero for course: {course.CourseName}. Skipping API call.");
                            continue;
                        }

                        // Update course balance using API endpoint
                        var updateBalanceUrl = $"http://balanceportal.runasp.net/api/Course/courses/{course.Id}/update-balance";
                        var courseBalanceUpdateRequest = new HttpRequestMessage(HttpMethod.Patch, updateBalanceUrl)
                        {
                            Content = new StringContent(JsonSerializer.Serialize(new { newBalance = course.CourseBalance }), Encoding.UTF8, "application/json")
                        };

                        var courseBalanceResponse = await _httpClient.SendAsync(courseBalanceUpdateRequest);

                        if (!courseBalanceResponse.IsSuccessStatusCode)
                        {
                            _logger.LogError($"Failed to update course balance for course ID {course.Id}. Status code: {courseBalanceResponse.StatusCode}");
                        }

                        // Add professor transactions using API endpoint
                        var professors = await _context.ProfessorCourses
                            .Include(pc => pc.Professor)
                            .Where(pc => pc.CourseId == course.Id)
                            .ToListAsync();

                        foreach (var professorCourse in professors)
                        {
                            if (professorCourse.Professor == null)
                            {
                                _logger.LogError($"Professor is null for ProfessorCourse with Course ID {professorCourse.CourseId}. Skipping transaction.");
                                continue;
                            }

                            var professorBalanceAddition = courseBalanceAddition * professorCourse.ProfShare;

                            var transactionDTO = new ProfessorTransactionDTO
                            {
                                NationalId = professorCourse.Professor.NationalID,
                                MobileNumber = professorCourse.Professor.PhoneNumber,
                                TransactionReference = _random.Next(1, 1000000).ToString(),
                                Reference2 = _random.Next(1, 1000000).ToString(),
                                TransactionAmount = professorBalanceAddition,

                                Type = TransactionType.Credit,
                                Status = TransactionStatus.New
                            };

                            var addTransactionUrl = "http://balanceportal.runasp.net/api/ProfessorTransaction/transactions";
                            var addTransactionRequest = new HttpRequestMessage(HttpMethod.Post, addTransactionUrl)
                            {
                                Content = new StringContent(JsonSerializer.Serialize(transactionDTO), Encoding.UTF8, "application/json")
                            };

                            var transactionResponse = await _httpClient.SendAsync(addTransactionRequest);

                            if (!transactionResponse.IsSuccessStatusCode)
                            {
                                _logger.LogError($"Failed to add professor transaction for professor with National ID {transactionDTO.NationalId}. Status code: {transactionResponse.StatusCode}");
                            }

                       
                        }
                    }
                }

                foreach (var record in newRecords)
                {
                    record.IsCalculated = true;
                }

                await _context.SaveChangesAsync();



                return Ok("File uploaded, processed, and calculated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred during file upload and processing: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during file processing.");
            }
        }

        [HttpGet("getAllUniversityExpenses")]
        public async Task<IActionResult> GetAllUniversityExpenses()
        {
            try
            {
                var universityExpenses = await _context.UniversityExpenses.ToListAsync();
                return Ok(universityExpenses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve UniversityExpenses data: {ex.Message}");
                return BadRequest($"Failed to retrieve UniversityExpenses data: {ex.Message}");
            }
        }

        [HttpDelete("deleteUniversityExpenses")]
        public async Task<IActionResult> DeleteUniversityExpenses()
        {
            try
            {
                // Ensure you only delete records from UniversityExpense table
                _context.UniversityExpenses.RemoveRange(_context.UniversityExpenses);

                await _context.SaveChangesAsync();

                return Ok("All records from UniversityExpense table have been deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete records from UniversityExpense table: {ex.Message}");
                return BadRequest($"Failed to delete records from UniversityExpense table: {ex.Message}");
            }
        }
    }
}
