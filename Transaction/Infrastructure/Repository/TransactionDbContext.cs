namespace Transaction.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using Transaction.Core.Models;

    public class TransactionDbContext : DbContext
    {

        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) { }

        public DbSet<Transaction> transactions { get; set; }
    }
}
