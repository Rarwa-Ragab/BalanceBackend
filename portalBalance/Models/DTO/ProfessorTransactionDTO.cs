using System;
using System.Text.Json.Serialization;

namespace portalBalance.Models.DTO
{
    public enum TransactionStatus
    {
        New,
        Confirmed,
        Approved
    }

    public enum TransactionType
    {
        Credit,
        Debit
    }

    public class ProfessorTransactionDTO
    {
        public int Id { get; set; }
        public string NationalId { get; set; }
        public string? MobileNumber { get; set; }
        public DateTime Date { get; set; }
        public string TransactionReference { get; set; }
        public string Reference2 { get; set; }
        public decimal TransactionAmount { get; set; }

        [JsonConverter(typeof(TransactionTypeConverter))]
        public TransactionType Type { get; set; }

     
        public string? UniversityName { get; set; }
        public string? FacultyName { get; set; }

        [JsonConverter(typeof(TransactionStatusConverter))]
        public TransactionStatus Status { get; set; }

        public int ProfessorId { get; set; }
        public bool NotificationSent { get; set; }
        public string? ProfessorName { get; set; }
    }
}
