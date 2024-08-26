namespace portalBalance.Models
{
    public class PaymentFile
    {
        public int Id { get; set; }
        public byte[] File { get; set; }  
        public string Organization { get; set; }
        public string UploaderName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public decimal TotalIncome { get; set; }
        public string Status { get; set; }
    }
}
