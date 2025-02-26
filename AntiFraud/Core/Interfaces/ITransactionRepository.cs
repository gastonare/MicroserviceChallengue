using AntiFraud.Core.Models;

namespace AntiFraud.Core.Interfaces
{
    public interface ITransactionRepository
    {
        List<Transaction> GetTransactions();
    }
}
