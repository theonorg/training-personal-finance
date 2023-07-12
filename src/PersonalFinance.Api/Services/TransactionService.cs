namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Services;

public class TransactionService : ITransactionService
{
    private readonly ILogger<TransactionService> _logger;
    private readonly PersonalFinanceDbContext _context;

    public TransactionService(PersonalFinanceDbContext context, ILogger<TransactionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    private static IExchangeRateCalculator SelectExchangeRateCalculator(string accountCurrency) => accountCurrency switch
    {
        "EUR" => new EuroExchangeRateCalculator(),
        "USD" => new USDExchangeRateCalculator(),
        "GBP" => new GBPExchangeRateCalculator(),
        _ => throw new ArgumentException($"Currency {accountCurrency} is not supported.")
    };

    public async Task<TransactionDTO> AddTransactionAsync(int accountId, string description, string currency, decimal amount, DateTime transactionDate)
    {
        _logger.LogInformation("Adding transaction with description {description}", description);
        Currency? currencyDB = await _context.Currencies.Where(c => c.Code == currency).FirstOrDefaultAsync();

        if (currencyDB == null)
        {
            _logger.LogWarning("Currency with code {currency} not found", currency);
            throw new ArgumentException("Currency with code {currency} not found", currency);
        }

        Account? account = await _context.Accounts.FindAsync(accountId);

        if (account == null)
        {
            _logger.LogWarning("Account with id {accountId} not found", accountId);
            throw new ArgumentException("Account with id {accountId} not found", accountId.ToString());
        }

        var exchangeRateCalculator = SelectExchangeRateCalculator(account.Currency.Code);
        decimal amountInAccountCurrency = exchangeRateCalculator.Convert(amount, currencyDB.Code);

        var transaction = new Transaction(description, currencyDB, account, amount, amountInAccountCurrency, transactionDate);
        _context.Transactions.Add(transaction);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Exception adding transaction {description}", description);
            throw;
        }

        TransactionDTO? returnValue = new(transaction.Id, transaction.Description, transaction.Currency.Code, transaction.Amount, transaction.AmountInAccountCurrency, transaction.TransactionDate);
        _logger.LogInformation("Returning transaction with id {id}", returnValue.Id);

        return returnValue;
    }

    public async Task<TransactionDTO> DeleteTransactionByIdAsync(int id)
    {
        _logger.LogInformation("Deleting transaction with id {id}", id);

        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction == null)
        {
            _logger.LogInformation("Transaction with id {id} not found", id);
            throw new ArgumentException($"Transaction with id {id} not found");
        }

        _context.Transactions.Remove(transaction);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Exception deleting transaction with id {id}", id);
            throw;
        }

        TransactionDTO? returnValue = new(transaction.Id, transaction.Description, transaction.Currency.Code, transaction.Amount, transaction.AmountInAccountCurrency, transaction.TransactionDate);
        return returnValue;
    }

    public async Task<TransactionDTO> GetTransactionByIdAsync(int id)
    {
        _logger.LogInformation("Getting transaction with id {id}", id);
        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction == null)
        {
            _logger.LogInformation("Transaction with id {id} not found", id);
            throw new ArgumentException($"Transaction with id {id} not found");
        }

        await _context.Entry(transaction).Reference("Currency").LoadAsync();   

        _logger.LogInformation("Returning transaction with id {id}", id);
        return new TransactionDTO(transaction.Id, transaction.Description, transaction.Currency.Code, transaction.Amount, transaction.AmountInAccountCurrency, transaction.TransactionDate);
    }

    public async Task<IEnumerable<TransactionDTO>> GetTransactionsAsync()
    {
        _logger.LogInformation("Getting all transactions");
        List<Transaction> all = await _context.Transactions.Include("Currency").OrderBy(tr => tr.TransactionDate).ToListAsync();
        IEnumerable<TransactionDTO> returnList = all.Select(tr => new TransactionDTO(tr.Id, tr.Description, tr.Currency.Code, tr.Amount, tr.AmountInAccountCurrency, tr.TransactionDate));
        _logger.LogInformation("Returning {count} accounts", returnList.Count());
        return returnList;
    }

    public async Task<IEnumerable<TransactionDTO>> GetTransactionsByMonthAsync(int year, int month)
    {
        _logger.LogInformation("Getting all transactions for year {year} and month {month}", year, month);
        List<Transaction> all = await _context.Transactions.Include("Currency").Where(tr => tr.TransactionDate.Year == year && tr.TransactionDate.Month == month).OrderBy(tr => tr.TransactionDate).ToListAsync();
        List<TransactionDTO> returnList = all.Select(tr => new TransactionDTO(tr.Id, tr.Description, tr.Currency.Code, tr.Amount, tr.AmountInAccountCurrency, tr.TransactionDate)).ToList();
        _logger.LogInformation("Returning {count} accounts", returnList.Count);
        return returnList;
    }

    public async Task<TransactionDTO> UpdateTransactionByIdAsync(int id, string description, string currency, decimal amount, DateTime transactionDate)
    {
        _logger.LogInformation("Updating transaction with id {id}", id);
        Currency? currencyDB = await _context.Currencies.Where(c => c.Code == currency).FirstOrDefaultAsync();

        if (currencyDB == null)
        {
            _logger.LogWarning("Currency with code {currency} not found", currency);
            throw new ArgumentException("Currency with code {currency} not found", currency);
        }

        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction == null)
        {
            _logger.LogInformation("Transaction with id {id} not found", id);
            throw new ArgumentException($"Transaction with id {id} not found");
        }

        transaction.Description = description;
        transaction.Currency = currencyDB;
        transaction.Amount = amount;
        transaction.TransactionDate = transactionDate;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Exception updating transaction with id {id}", id);
            throw;
        }

        TransactionDTO? returnValue = new(transaction.Id, transaction.Description, transaction.Currency.Code, transaction.Amount, transaction.AmountInAccountCurrency, transaction.TransactionDate);
        _logger.LogInformation("Returning transaction with id {id}", returnValue.Id);

        return returnValue;
    }
}
