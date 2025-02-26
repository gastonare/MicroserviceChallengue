using TransactionModel = Transaction.Core.Models.Transaction;

namespace Transaction.Core.Interfaces
{
    public interface ITransactionRepository
    {
        TransactionModel InsertRecord(TransactionModel transaction);
        TransactionModel UpdateRecord(TransactionModel transaction);
        TransactionModel? GetRecord(int id);
    }
}
