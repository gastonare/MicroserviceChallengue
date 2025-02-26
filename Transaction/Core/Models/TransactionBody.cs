namespace Transaction.Core.Models
{
    public class TransactionBody
    {
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public int Value { get; set; }
    }
}
