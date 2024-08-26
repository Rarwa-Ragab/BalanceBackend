using System;

namespace portalBalance.Models
{
    public class CourseTransaction
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime Date { get; set; }
        public string CourseName { get; set; }
        public int TransactionCount { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public string UniversityName { get; set; }
        public string FacultyName { get; set; }
        public string DepartmentName { get; set; }
        public int AcademicYear { get; set; }
    }
}
