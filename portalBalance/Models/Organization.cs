
using System.Text.Json.Serialization;

namespace portalBalance.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Org_Name { get; set; }
        public string OrgType { get; set; }
        public string BankAcount { get; set; }
        public string License_ID { get; set; }
        public string Momkn_ID { get; set; }
        public string Momkn_Financial_ID { get; set; }
        public bool State { get; set; }
        public float UniveristyCut { get; set; }
        [JsonIgnore]
        public List<SubOrg> SubOrgs { get; set; }
        [JsonIgnore]
        public List<OrgAdmin> OrgAdmins { get; set; }
    }
}
