namespace portalBalance.Models.DTO
{
    public class SubOrgDTO
    {
        public string ParentOrg_Name { get; set; }
        public string SubOrg_Name { get; set; }
        public string OrgType { get; set; }
        public string BankAcount { get; set; }
        public string License_ID { get; set; }
        public string Momkn_ID { get; set; }
        public string Momkn_Financial_ID { get; set; }
        public bool State { get; set; }
        public int OrganizationId { get; set; }
    }
}
