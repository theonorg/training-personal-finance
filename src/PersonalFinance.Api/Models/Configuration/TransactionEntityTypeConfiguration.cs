
namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Models.Configuration;

public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(acc => acc.Id);

        builder.Property(acc => acc.TransactionDate)
            .IsRequired();

        builder.Property(acc => acc.CurrencyId)
            .IsRequired();

        builder.Property(acc => acc.AccountId)
            .IsRequired();

        builder.Property(acc => acc.Description)
            .HasMaxLength(500);

        builder.Property(acc => acc.Amount)
            .IsRequired()
            .HasPrecision(10, 2);
        
        builder.Property(acc => acc.AmountInAccountCurrency)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(acc => acc.CreatedOn)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}