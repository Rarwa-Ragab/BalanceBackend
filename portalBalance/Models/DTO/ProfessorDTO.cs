namespace portalBalance.Models.DTO
{
    public class ProfessorDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string NationalID { get; set; }
        public string PhoneNumber { get; set; }
        public bool State { get; set; }
        public decimal Professorbalance { get; set; }
        public string Org_Name { get; set; }
        public string Sub_Org_Name { get; set; }
        public List<CourseDTO> Courses { get; set; } = new List<CourseDTO>(); 
    }
}
