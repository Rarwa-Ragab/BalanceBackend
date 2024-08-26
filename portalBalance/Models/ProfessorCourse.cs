namespace portalBalance.Models
{
    public class ProfessorCourse
    {
        public int ProfessorId { get; set; }
        public Professor Professor { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public decimal ProfShare { get; set; }
    }
}
