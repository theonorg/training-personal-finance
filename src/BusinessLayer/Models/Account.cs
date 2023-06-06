using Theonorg.DotnetTraining.ExchangeRateLib;

namespace Theonorg.DotnetTraining.BusinessLayer.Models;

public class Account
{
    internal Currency AccountCurrency { get; }
    internal string AccountName { get; set; }
    internal decimal InitialBalance { get; }
    internal decimal Balance { get; set; }
    List<Transaction> Transactions { get; } = new List<Transaction>();
    internal IExchangeRateCalculator ExchangeRateCalculator { get; }

    internal Account(string accountName, Currency accountCurrency, decimal initialBalance) {
        AccountName = accountName;
        AccountCurrency = accountCurrency;
        InitialBalance = initialBalance;
        Balance = initialBalance;
        ExchangeRateCalculator = SelectExchangeRateCalculator(accountCurrency);
    }

    private IExchangeRateCalculator SelectExchangeRateCalculator(Currency accountCurrency) => accountCurrency switch
    {
        Currency.EUR => new EuroExchangeRateCalculator(),
        Currency.USD => new USDExchangeRateCalculator(),
        Currency.GBP => new GBPExchangeRateCalculator(),
        _ => throw new ArgumentException($"Currency {accountCurrency} is not supported.")
    };

    internal void AddTransaction(Transaction transaction)
    {
        Balance += transaction.AmountInAccountCurrency;
        Transactions.Add(transaction);
    }

    internal decimal GetBalance() => Balance;

    internal List<Transaction> GetTransactionsByMonth(int year, int month) => Transactions.Where(t => t.TransactionDate.Year == year && t.TransactionDate.Month == month).ToList();

    internal List<Transaction> GetTransactionsBetweenDates(DateTime startDate, DateTime endDate) => Transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).ToList(); 

    public override string ToString() => $"{AccountName} - {AccountCurrency} {Balance}";
}
