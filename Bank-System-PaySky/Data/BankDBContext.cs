using Microsoft.EntityFrameworkCore;
using Bank_System_PaySky.Entities.TransactionsModels;
using Bank_System_PaySky.Entites.AccountModdels;
using Bank_System_PaySky.Entities.AccountModdels;
using Bank_System_PaySky.Entities.AccountTransactionsModels;

namespace Bank_System_PaySky.Data
{
    public class BankingDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<AccountTransactions> AccountTransactions { get; set; }

        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasDiscriminator<string>("AccountType")
                .HasValue<SavingAccount>("Savings")
                .HasValue<CheckingAccount>("Checking");

            modelBuilder.Entity<Account>()
                .Property(a => a.AccountId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Account>()
               .Property(a => a.Balance)
               .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<CheckingAccount>()
               .Property(c => c.Overdrafts)
               .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<SavingAccount>()
                .Property(c => c.Interest)
                .HasColumnType("decimal(18, 2)");


            modelBuilder.Entity<Transaction>()
                .Property(t => t.TransactionId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<AccountTransactions>()
                .HasKey(at => new { at.AccountId, at.TransactionId });

            modelBuilder.Entity<AccountTransactions>()
                .HasOne(at => at.Account)
                .WithMany(a => a.AccountTransactions)
                .HasForeignKey(at => at.AccountId);

            modelBuilder.Entity<AccountTransactions>()
                .HasOne(at => at.Transaction)  
                .WithMany(t => t.AccountTransactions)
                .HasForeignKey(at => at.TransactionId);
        }
    }
}
