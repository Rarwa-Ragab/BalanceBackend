using System.Collections.Generic;

namespace portalBalance.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string DepartmentName { get; set; }
        public string universityName { get; set; }
        public string FaculityName { get; set; }
        public string EducationYear { get; set; }
        public string EducationTerm { get; set; }
        public int CourseHours { get; set; }
        public decimal PricePerHour { get; set; }
        public bool State { get; set; }
        public decimal CourseBalance { get; set; }
        public DateTime LastUpdate { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public List<ProfessorCourse> ProfessorCourses { get; set; } = new List<ProfessorCourse>();
        public List<CourseTransaction> Transactions { get; set; } = new List<CourseTransaction>();
    }
}
