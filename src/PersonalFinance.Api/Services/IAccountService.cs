
namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Services;

    public interface IAccountService
{
    public Task<AccountDTO> AddAsync(string name, string currency, decimal initialBalance);
    public Task<IEnumerable<AccountDTO>> GetAllAsync();
    public Task<AccountDTO> GetByIdAsync(int id);
    public Task<SimpleAccountDTO> DeleteByIdAsync(int id);
    public Task<AccountDTO> UpdateByIdAsync(int id, string name);
}

