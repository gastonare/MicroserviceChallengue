namespace AntiFraud.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public int Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; } // 1 = pending, 2 = approved, 3 = rejected
        public Guid TransactionExternalId { get; set; }
    }
}
