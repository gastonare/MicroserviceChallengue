using AntiFraud.Core.Models;

namespace AntiFraud.Core.Interfaces
{
    public interface IAntiFraudService
    {
        public Task<TransactionResponse> CheckTransactions();
    }
}
