namespace portalBalance.Models
{
    public class UniversityExpense
    {
        public int Id { get; set; }
        public int BillTypeCode { get; set; }
        public long BillReferenceNo { get; set; }
        public string BillTypeName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Time { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public long NationalId { get; set; }
        public long StudentCode { get; set; }
        public string StudentName { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal TuitionFees { get; set; }
        public decimal BookFees { get; set; }
        public string ReconStatus { get; set; }
        public string FacultyCode { get; set; }
        public string FacultyName { get; set; }
        public string AcademicYear { get; set; }
        public string ClassSemester { get; set; }
        public string StudyNature { get; set; }
        public string AsNode { get; set; }
        public string PhaseNode { get; set; }
        public bool IsCalculated { get; set; } 
    }

}
