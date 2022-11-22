namespace Findox.Application.Services.Account
{
    using Findox.Application.Dto.Account;
    using Findox.Domain.Entities;

    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> GetByIdAsync(int id);
        Task<Account?> CreateAsync(CreateAccountRequest request);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, UpdateAccountRequest request);
    }
}