namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Services;

public interface ITransactionService
{
    public Task<TransactionDTO> AddTransactionAsync(int accountId, string description, string currency, decimal amount, DateTime transactionDate);
    public Task<IEnumerable<TransactionDTO>> GetTransactionsAsync();
    public Task<TransactionDTO> GetTransactionByIdAsync(int id);
    public Task<TransactionDTO> DeleteTransactionByIdAsync(int id);
    public Task<TransactionDTO> UpdateTransactionByIdAsync(int id, string description, string currency, decimal amount, DateTime transactionDate);
    public Task<IEnumerable<TransactionDTO>> GetTransactionsByMonthAsync(int year, int month);
}
