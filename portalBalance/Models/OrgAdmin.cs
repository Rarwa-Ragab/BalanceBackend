
using System.Text.Json.Serialization;

namespace portalBalance.Models
{
    public class OrgAdmin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string NationalID { get; set; }
        public string Phonenumber { get; set; }
        public string BusinessID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string org_Name { get; set; }
        public int OrganizationId { get; set; }
     
        [JsonIgnore]
        public Organization Organization { get; set; } 





    }
}
