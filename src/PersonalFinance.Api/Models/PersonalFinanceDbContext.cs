namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Models;

public class PersonalFinanceDbContext: DbContext
{
    public PersonalFinanceDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new AccountEntityTypeConfiguration().Configure(builder.Entity<Account>());
        new CurrencyEntityTypeConfiguration().Configure(builder.Entity<Currency>());
        new TransactionEntityTypeConfiguration().Configure(builder.Entity<Transaction>());
    }
}
