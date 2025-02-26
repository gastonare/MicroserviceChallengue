namespace Transaction.Core.Models
{
    public class TransactionResponse
    {
        public string MessageError { get; set; }
        public bool IsSuccess { get; set; }

        public Guid TransactionExternalId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
