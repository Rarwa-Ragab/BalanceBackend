using System.Collections.Generic;

namespace portalBalance.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string NationalID { get; set; }
        public string PhoneNumber { get; set; }
        public bool State { get; set; }
        public string Org_Name { get; set; }
        public string Sub_Org_Name { get; set; }
        public decimal Professorbalance { get; set; }

        public List<ProfessorCourse> ProfessorCourses { get; set; } = new List<ProfessorCourse>();
        public List<ProfessorTransaction> ProfessorTransactions { get; set; } = new List<ProfessorTransaction>();  // New relationship
    }
}
