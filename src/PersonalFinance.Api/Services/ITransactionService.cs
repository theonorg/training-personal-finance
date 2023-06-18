using Theonorg.DotnetTraining.BusinessLayer.Models;

namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Services;

public interface ITransactionService
{
    public TransactionModel Add(string description, Currency currency, decimal amount, DateTime transactionDate);
    public List<TransactionModel> GetTransactions();
    public TransactionModel GetTransactionById(int id);
    public void DeleteTransactionById(int id);
    public void UpdateTransactionById(int id, string description, Currency currency, decimal amount, DateTime transactionDate);
    public List<TransactionModel> GetTransactionsByMonth(int year, int month);
}
