namespace portalBalance.Models.DTO
{
    public class PaymentFileDTO
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }  
        public string OrganizationName { get; set; }
        public string UploaderName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public decimal TotalIncome { get; set; }
        public string Status { get; set; }
    }
}
