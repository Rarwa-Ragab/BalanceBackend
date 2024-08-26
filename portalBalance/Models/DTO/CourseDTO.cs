namespace portalBalance.Models.DTO
{
    public class CourseDTO
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
        public List<ProfessorCourseDTO> Professors { get; set; } = new List<ProfessorCourseDTO>();
    }
}
