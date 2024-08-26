using System.Collections.Generic;

namespace portalBalance.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Department_Name { get; set; }
        public string Org_Name { get; set; }
        public string Sub_Org_Name { get; set; }
        public bool State { get; set; }
        public int SubOrgId { get; set; }
        public SubOrg SubOrg { get; set; }

        public List<Course> Courses { get; set; }
    }
}
