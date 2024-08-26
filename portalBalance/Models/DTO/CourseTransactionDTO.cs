namespace portalBalance.Models.DTO
{
    public class CourseTransactionDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime Date { get; set; }
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
