namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Services;

public class AccountService : IAccountService
{
    private readonly PersonalFinanceDbContext _context;
    private readonly ILogger _logger;

    public AccountService(PersonalFinanceDbContext context, ILogger<AccountService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<AccountDTO>> GetAllAsync()
    {
        _logger.LogInformation("Getting all accounts");
        List<Account> allAccounts = await _context.Accounts.Include(acc => acc.Currency).OrderBy(acc => acc.Id).ToListAsync();
        IEnumerable<AccountDTO> returnList = allAccounts.Select(acc => new AccountDTO(acc.Id, acc.Name, acc.Currency.Code, acc.Balance));
        _logger.LogInformation("Returning {count} accounts", returnList.Count());
        return returnList;
    }

    public async Task<AccountDTO> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting account with id {id}", id);
        var account = await _context.Accounts.Include("Currency").SingleOrDefaultAsync(acc => acc.Id == id);

        if (account == null)
        {
            _logger.LogInformation("Account with id {id} not found", id);
            throw new ArgumentException($"Account with id {id} not found");
        }
        _logger.LogInformation("Returning account with id {id}", id);
        return new AccountDTO(account.Id, account.Name, account.Currency.Code, account.Balance);
    }

    public async Task<AccountDTO> AddAsync(string name, string currency, decimal initialBalance)
    {
        _logger.LogInformation("Adding account {name}, {currency}, {initialBalance}", name, currency, initialBalance);
        Currency? currencyDB = await _context.Currencies.Where(c => c.Code == currency).FirstOrDefaultAsync();

        if (currencyDB == null)
        {
            _logger.LogWarning("Currency with code {currency} not found", currency);
            throw new ArgumentException("Currency with code {currency} not found", currency);
        }

        Account account = new(currencyDB, name, initialBalance);
        _context.Accounts.Add(account);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Exception adding account {name}", name);
            throw;
        }

        AccountDTO? returnValue = new(account.Id, account.Name, account.Currency.Code, account.Balance);
        return returnValue;
    }

    public async Task<SimpleAccountDTO> DeleteByIdAsync(int id)
    {
        _logger.LogInformation("Deleting account with id {id}", id);
        Account? account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            _logger.LogInformation("Account with id {id} not found", id);
            throw new ArgumentException($"Account with id {id} not found");
        }

        _context.Accounts.Remove(account);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Exception deleting account with id {id}", id);
            throw;
        }

        SimpleAccountDTO? returnValue = new(account.Id, account.Name);
        return returnValue;
    }

    public async Task<AccountDTO> UpdateByIdAsync(int id, string name)
    {
        _logger.LogInformation("Updating account with id {id}", id);
        Account? account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            _logger.LogInformation("Account with id {id} not found", id);
            throw new ArgumentException($"Account with id {id} not found");
        }

        account.Name = name;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Exception updating account with id {id}", id);
            throw;
        }

        account = await _context.Accounts.Include("Currency").SingleOrDefaultAsync(acc => acc.Id == id);

        AccountDTO? returnValue = new(account.Id, account.Name, account.Currency.Code, account.Balance);
        return returnValue;
    }
}
