
using Theonorg.DotnetTraining.BusinessLayer.Models;

namespace Theonorg.DotnetTraining.BusinessLayer.Services;

public class AccountService
{
    public Account Account { get; }

    public AccountService(string accountName, Currency accountCurrency, decimal initialBalance)
    {
        Account = new Account(accountName, accountCurrency, initialBalance);
    }

    public void AddExpense(string description, Currency currency, decimal amount, DateTime transactionDate)
    {
        if (amount > 0)
        {
            amount = -amount;
        }

        Expense expense = new Expense(description, currency, amount, Account.ExchangeRateCalculator.Convert(amount, currency.ToString()), transactionDate);

        Account.AddTransaction(expense);
    }

    public void AddIncome(string description, Currency currency, decimal amount, DateTime transactionDate)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }

        Income income = new Income(description, currency, amount, Account.ExchangeRateCalculator.Convert(amount, currency.ToString()), transactionDate);

        Account.AddTransaction(income);
    }

    public string GetBalance() => $"{Account.AccountCurrency} {Account.GetBalance()}";

    public List<Transaction> GetTransactionsByMonth(int year, int month) => Account.GetTransactionsByMonth(year, month);

    public List<Transaction> GetTransactionsBetweenDates(DateTime startDate, DateTime endDate) => Account.GetTransactionsBetweenDates(startDate, endDate);

}
