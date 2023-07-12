
namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Models.Configuration;

public class CurrencyEntityTypeConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(cur => cur.Id);
        builder.HasIndex(cur => cur.Code).IsUnique(true);

        builder.Property(cur => cur.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cur => cur.Code)
            .IsRequired()
            .HasMaxLength(3);

        builder.HasData(
            new { Id = 1, Name = "Euro", Code = "EUR" },
            new { Id = 2, Name = "Dollar", Code = "USD" },
            new { Id = 3, Name = "British Pound", Code = "GBP" }
        );
    }
}