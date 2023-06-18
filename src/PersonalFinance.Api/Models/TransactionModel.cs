

namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Models;

public record TransactionModel : Transaction
{
    public TransactionModel(int id, string Description, Currency Currency, decimal Amount, decimal AmountInAccountCurrency, DateTime TransactionDate) : 
        base(Description, Currency, Amount, AmountInAccountCurrency, TransactionDate)
    {
        Id = id;
    }

    public int Id { get; init; }
}
