using Microsoft.EntityFrameworkCore;
using Bank_System_PaySky.Entities.TransactionsModels;
using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Entities.AccountTransactionsModels;
using Bank_System_PaySky.Entities.UserModels;
using Bank_System_PaySky.Entities.CurrencyModel;

namespace Bank_System_PaySky.Data
{
    public class BankingDbContext : DbContext
    {
        // DbSet for accounts
        public DbSet<Account> Accounts { get; set; }

        // DbSet for transactions
        public DbSet<Transaction> Transactions { get; set; }

        // DbSet for account transactions
        public DbSet<AccountTransactions> AccountTransactions { get; set; }
        // DbSet for users
        public DbSet<Users> Users { get; set; }
        // DbSet for currencies
        public DbSet<Currency> Currencies { get; set; }

        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Account entity (Single Table Inheritance for CheckingAccount and SavingAccount)
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

            // Configure CheckingAccount entity
            modelBuilder.Entity<CheckingAccount>()
                .Property(c => c.Overdrafts)
                .HasColumnType("decimal(18, 2)");

            // Configure SavingAccount entity
            modelBuilder.Entity<SavingAccount>()
                .Property(c => c.Interest)
                .HasColumnType("decimal(18, 2)");

            // Configure Transaction entity
            modelBuilder.Entity<Transaction>()
                .Property(t => t.TransactionId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18, 2)");

            // Configure AccountTransaction entity (Many-to-Many Relationship)
            modelBuilder.Entity<AccountTransactions>()
                .HasKey(at => new { at.AccountId, at.TransactionId });

            modelBuilder.Entity<AccountTransactions>()
                .HasOne(at => at.Account)
                .WithMany(a => a.AccountTransactions)
                .HasForeignKey(at => at.AccountId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when Account is deleted

            modelBuilder.Entity<AccountTransactions>()
                .HasOne(at => at.Transaction)
                .WithMany(t => t.AccountTransactions)
                .HasForeignKey(at => at.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when Transaction is deleted

            // Configure Users entity
            modelBuilder.Entity<Users>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<Users>()
                .Property(u => u.UserId)
                .ValueGeneratedOnAdd();
                

            modelBuilder.Entity<Users>()
                .Property(u => u.Username)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Users>()
                .Property(u => u.Email)
                .HasMaxLength(200)
                .IsRequired();

            // Configure Currency entity
            modelBuilder.Entity<Currency>()
                .HasKey(c => c.CurrencyCode);

            modelBuilder.Entity<Currency>()
                .Property(c => c.CurrencyCode)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<Currency>()
                .Property(c => c.ExchangeRate)
                .HasColumnType("decimal(18, 6)")
                .IsRequired();

            modelBuilder.Entity<Currency>()
                .Property(c => c.IsBase)
                .IsRequired();

            // Configure the relationship between Users and Accounts
            modelBuilder.Entity<Account>()
                .HasOne(a => a.User) // Each account has a single user
                .WithMany(u => u.Accounts) // A user can have multiple accounts
                .HasForeignKey(a => a.UserId) // Foreign key in Account
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete: if a user is deleted, all their accounts will be deleted

            // Configure the relationship between Account and Currency
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Currency)         // An Account has one Currency
                .WithMany(c => c.Accounts)       // A Currency can have many Accounts
                .HasForeignKey(a => a.CurrencyCode)
                .OnDelete(DeleteBehavior.Restrict);  // Do not delete Accounts when Currency is deleted

            //Configure the relationship between Transaction and Currency
            modelBuilder.Entity<AccountTransactions>()
                .HasOne(t => t.Currency)
                .WithMany(c => c.AccountTransactions)
                .HasForeignKey(t => t.CurrencyCode)
                .OnDelete(DeleteBehavior.Restrict);  // Do not delete Transactions when Currency is deleted

            // Seed data for Currency table with updated exchange rates
            modelBuilder.Entity<Currency>().HasData(
                new Currency { CurrencyCode = "USD", ExchangeRate = 1.0m, IsBase = true }, // US Dollar // Base Currency
                new Currency { CurrencyCode = "EUR", ExchangeRate = 0.96m, IsBase = false }, // Euro
                new Currency { CurrencyCode = "GBP", ExchangeRate = 0.80m, IsBase = false }, // British Pound
                new Currency { CurrencyCode = "EGP", ExchangeRate = 50.86m, IsBase = false }, // Egyptian Pound
                new Currency { CurrencyCode = "SAR", ExchangeRate = 3.76m, IsBase = false }  // Saudi Riyal
            );

        }

    }
}
