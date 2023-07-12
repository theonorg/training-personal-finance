namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Models.Configuration;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(acc => acc.Id);
        builder.HasIndex(acc => acc.Name).IsUnique(true);

        builder.Property(acc => acc.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(acc => acc.InitialBalance)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(acc => acc.Balance)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(acc => acc.CreatedOn)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

    }
}