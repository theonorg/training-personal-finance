namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Services;

public class TransactionService : ITransactionService
{
    private ConcurrentDictionary<int, TransactionModel> _transactions = new ConcurrentDictionary<int, TransactionModel>();
    private ILogger<TransactionService> _logger;
    private IExchangeRateCalculator _exchangeRateCalculator;
    private int _id = 0;

    public TransactionService(ILogger<TransactionService> logger)
    {
        _logger = logger;
        _exchangeRateCalculator = new EuroExchangeRateCalculator(); // using Euro as static calcultator
    }

    public TransactionModel Add(string description, Currency currency, decimal amount, DateTime transactionDate)
    {
        Interlocked.Increment(ref _id);
        TransactionModel transaction = new TransactionModel(_id, description, currency, amount, _exchangeRateCalculator.Convert(amount, currency.ToString()), transactionDate);

        _transactions.TryAdd(_id, transaction);
        _logger.LogInformation($"Added transaction: {transaction}");

        return transaction;
    }

    public void DeleteTransactionById(int id)
    {
        if (_transactions.TryRemove(id, out TransactionModel? transaction))
        {
            _logger.LogInformation($"Removed transaction: {transaction}");
        }
        else
        {
            _logger.LogInformation($"Transaction with id {id} not found");
        }
    }

    public TransactionModel GetTransactionById(int id)
    {
        _logger.LogInformation($"Get transaction with id {id}");
        if (_transactions.TryGetValue(id, out TransactionModel? transaction))
        {
            _logger.LogInformation($"Found transaction: {transaction}");
            return transaction;
        }
        else
        {
            _logger.LogInformation($"Transaction with id {id} not found");
            throw new ArgumentException($"Transaction with id {id} not found");
        }
    }

    public List<TransactionModel> GetTransactions()
    {
        _logger.LogInformation($"Get all transactions");
        return _transactions.Values.ToList();
    }

    public List<TransactionModel> GetTransactionsByMonth(int year, int month)
    {
        _logger.LogInformation($"Get transactions for year {year} and month {month}");
        return _transactions.Values.Where(t => t.TransactionDate.Year == year && t.TransactionDate.Month == month).ToList();
    }

    public void UpdateTransactionById(int id, string description, Currency currency, decimal amount, DateTime transactionDate)
    {
        _logger.LogInformation($"Update transaction with id {id}");
        if (_transactions.TryGetValue(id, out TransactionModel? transaction))
        {
            _logger.LogInformation($"Found transaction: {transaction}");
            var updated = new TransactionModel(id, description, currency, amount, _exchangeRateCalculator.Convert(amount, currency.ToString()), transactionDate);
            //_transactions.AddOrUpdate(_id, updated, (oldkey, oldvalue) => updated);
            _transactions[id] = updated;
            _logger.LogInformation($"Updated transaction: {transaction}");
        }
        else
        {
            _logger.LogInformation($"Transaction with id {id} not found");
            throw new ArgumentException($"Transaction with id {id} not found");
        }
    }
}
