namespace Findox.Infra.Data.Repositories.User
{
    using Findox.Domain.Entities;

    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account?> GetByUsernameAsync(string? username);
        Task<Account> GetByIdAsync(int id);
        Task<Account?> CreateAsync(Account account);
        Task UpdateAsync(int id, Account account);
        Task DeleteAsync(int id);
    }
}
