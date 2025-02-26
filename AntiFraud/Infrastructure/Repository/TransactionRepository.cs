using AntiFraud.Core.Interfaces;
using AntiFraud.Core.Models;

namespace AntiFraud.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _transactionDbContext;

        public TransactionRepository(TransactionDbContext transactionDbContext)
        {
            _transactionDbContext = transactionDbContext;
        }

        public List<Transaction> GetTransactions()
        {
            var result = _transactionDbContext.transactions.ToList();
            return result;
        }
    }
}
