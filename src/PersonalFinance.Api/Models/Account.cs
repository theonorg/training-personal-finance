
using System.Diagnostics.CodeAnalysis;

namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Models;

public class Account
{
    public int Id { get; set; }
    public Currency Currency { get; set; }
    public string Name { get; set; }
    public decimal InitialBalance { get; init; }
    public decimal Balance { get; set; }
    public List<Transaction> Transactions { get; } = new List<Transaction>();
    public DateTime CreatedOn { get; set; }

    public Account() {}

    public Account(Currency currency, string name, decimal initialBalance)
    {
        Currency = currency;
        Name = name;
        InitialBalance = initialBalance;
        Balance = initialBalance;
    }
}
