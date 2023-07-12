
namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Models;

public class Transaction
{
    public int Id { get; set; }
    public string Description { get; set; } 
    public Currency Currency { get; set; }
    public int CurrencyId { get; set; }
    public Account Account { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountInAccountCurrency { get; set; } 
    public DateTime TransactionDate { get; set; }
    public DateTime CreatedOn { get; set; }

    public Transaction() {}

    public Transaction(string description, Currency currency, Account account, decimal amount, decimal amountInAccountCurrency, DateTime transactionDate)
    {
        Description = description;
        Currency = currency;
        Account = account;
        Amount = amount;
        AmountInAccountCurrency = amountInAccountCurrency;
        TransactionDate = transactionDate;
    }
}
