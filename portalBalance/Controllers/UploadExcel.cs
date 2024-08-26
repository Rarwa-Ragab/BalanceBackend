using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using portalBalance.Models;
using portalBalance.Data;

namespace portalBalance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadExcelController : ControllerBase
    {
        private readonly PortalBalanceContext _context;

        public UploadExcelController(PortalBalanceContext context)
        {
            _context = context;
        }

        [HttpPost("upload-excel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);

                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                var processedDepartments = new HashSet<string>();
                var processedProfessors = new HashSet<string>();
                var courseDict = new Dictionary<string, Course>();

                for (int row = 2; row <= rowCount; row++)
                {
                    if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text))
                    {
                        continue;
                    }

                    var educationYear = worksheet.Cells[row, 1].Text.Trim();
                    var departmentName = worksheet.Cells[row, 2].Text.Trim();
                    var courseName = worksheet.Cells[row, 3].Text.Trim();
                    var courseHours = ParseInt(worksheet.Cells[row, 4].Text.Trim(), row, 4);
                    var pricePerHour = ParseDecimal(worksheet.Cells[row, 5].Text.Trim(), row, 5);
                    var profShare = ParseDecimal(worksheet.Cells[row, 6].Text.Trim(), row, 6); // Read directly as decimal
                    var username = worksheet.Cells[row, 7].Text.Trim();
                    var nationalID = worksheet.Cells[row, 8].Text.Trim();
                    var phoneNumber = worksheet.Cells[row, 9].Text.Trim();
                    var universityName = worksheet.Cells[row, 10].Text.Trim();
                    var facultyName = worksheet.Cells[row, 11].Text.Trim();
                    var educationTerm = worksheet.Cells[row, 12].Text.Trim();

                    var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.Org_Name == universityName);
                    if (organization == null)
                    {
                        organization = new Organization { Org_Name = universityName };
                        _context.Organizations.Add(organization);
                        await _context.SaveChangesAsync();
                    }

                    var subOrganization = await _context.SubOrgs.FirstOrDefaultAsync(so => so.SubOrg_Name == facultyName && so.ParentOrg_Name == universityName);
                    if (subOrganization == null)
                    {
                        subOrganization = new SubOrg { SubOrg_Name = facultyName, ParentOrg_Name = universityName, OrganizationId = organization.Id };
                        _context.SubOrgs.Add(subOrganization);
                        await _context.SaveChangesAsync();
                    }

                    var departmentKey = $"{departmentName}-{universityName}-{facultyName}";

                    if (!processedDepartments.Contains(departmentKey))
                    {
                        var department = await _context.Departments
                            .FirstOrDefaultAsync(d => d.Department_Name == departmentName && d.Org_Name == universityName && d.Sub_Org_Name == facultyName);

                        if (department == null)
                        {
                            department = new Department
                            {
                                Department_Name = departmentName,
                                Org_Name = universityName,
                                Sub_Org_Name = facultyName,
                                State = true,
                                SubOrgId = subOrganization.Id
                            };
                            _context.Departments.Add(department);
                            await _context.SaveChangesAsync();
                        }
                        processedDepartments.Add(departmentKey);
                    }

                    if (!processedProfessors.Contains(nationalID))
                    {
                        var professor = await _context.Professors.FirstOrDefaultAsync(p => p.NationalID == nationalID);
                        if (professor == null)
                        {
                            professor = new Professor
                            {
                                Username = username,
                                NationalID = nationalID,
                                PhoneNumber = phoneNumber,
                                Org_Name = universityName,
                                State = true,
                                Sub_Org_Name = facultyName
                            };
                            _context.Professors.Add(professor);
                            await _context.SaveChangesAsync();
                        }
                        processedProfessors.Add(nationalID);
                    }

                    var courseKey = $"{courseName}-{departmentName}-{universityName}-{facultyName}-{educationYear}-{educationTerm}";

                    if (!courseDict.ContainsKey(courseKey))
                    {
                        var departmentId = (await _context.Departments
                            .FirstOrDefaultAsync(d => d.Department_Name == departmentName && d.Org_Name == universityName && d.Sub_Org_Name == facultyName)).Id;

                        var course = new Course
                        {
                            CourseName = courseName,
                            DepartmentName = departmentName,
                            EducationYear = educationYear,
                            EducationTerm = educationTerm,
                            CourseHours = courseHours,
                            PricePerHour = pricePerHour,
                            DepartmentId = departmentId,
                            universityName = universityName,
                            FaculityName = facultyName,
                            LastUpdate = DateTime.UtcNow,
                            State = true,
                            ProfessorCourses = new List<ProfessorCourse>()
                        };

                        courseDict[courseKey] = course;
                    }

                    var professorEntity = await _context.Professors.FirstOrDefaultAsync(p => p.NationalID == nationalID);
                    var professorCourse = new ProfessorCourse
                    {
                        Course = courseDict[courseKey],
                        Professor = professorEntity,
                        ProfShare = profShare 
                    };

                    courseDict[courseKey].ProfessorCourses.Add(professorCourse);
                }

                foreach (var course in courseDict.Values)
                {
                    _context.Courses.Add(course);
                }

                await _context.SaveChangesAsync();
                return Ok("File uploaded and processed successfully.");
            }
            catch (FormatException ex)
            {
                return BadRequest($"Format error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private int ParseInt(string value, int row, int column)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new FormatException($"Empty or whitespace value at row {row}, column {column}");
            }
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }
            throw new FormatException($"Invalid integer value at row {row}, column {column}: {value}");
        }

        private decimal ParseDecimal(string value, int row, int column)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new FormatException($"Empty or whitespace value at row {row}, column {column}");
            }
            if (decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
            {
                return result; 
            }
            throw new FormatException($"Invalid decimal value at row {row}, column {column}: {value}");
        }
    }
}
