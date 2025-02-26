using Transaction.Core.Models;

namespace Transaction.Core.Interfaces
{
    public interface ITransactionService
    {
        public Task<TransactionResponse> CreateTransaction(TransactionBody transactionBody);
        public TransactionResponse UpdateTransactions();
    }
}
