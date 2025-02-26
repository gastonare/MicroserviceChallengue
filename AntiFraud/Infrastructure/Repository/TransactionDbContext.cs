using AntiFraud.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AntiFraud.Infrastructure.Repository
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) { }

        public DbSet<Transaction> transactions { get; set; }
    }
}
