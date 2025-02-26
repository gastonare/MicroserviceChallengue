using Microsoft.EntityFrameworkCore;
using Transaction.Core.Interfaces;
using TransactionModel = Transaction.Core.Models.Transaction;

namespace Transaction.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _transactionDbContext;

        public TransactionRepository(TransactionDbContext transactionDbContext)
        {
            _transactionDbContext = transactionDbContext;
        }

        public TransactionModel? GetRecord(int id)
        {
            var transaction = _transactionDbContext.transactions.FirstOrDefault(x => x.Id == id);
            return transaction ?? null;
        }

        public TransactionModel InsertRecord(TransactionModel transaction)
        {
            var createdTransaction = _transactionDbContext.transactions.Add(transaction);
            _transactionDbContext.SaveChanges();
           
            return createdTransaction.Entity;
        }

        public TransactionModel UpdateRecord(TransactionModel transaction)
        {
            var updatedTransaction = _transactionDbContext.transactions.Update(transaction);
            _transactionDbContext.SaveChanges();

            return updatedTransaction.Entity;
        }
    }
}
