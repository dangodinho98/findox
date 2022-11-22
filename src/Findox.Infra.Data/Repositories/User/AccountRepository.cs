
namespace Findox.Infra.Data.Repositories.User
{
    using Dapper;
    using Findox.Domain.Entities;
    using Findox.Shared.Extensions;
    using Microsoft.Extensions.Configuration;
    using Npgsql;
    using System.Data;

    public class AccountRepository : BaseRepository, IAccountRepository
    {
        private const string TableName = "accounts";

        public AccountRepository(IConfiguration configuration)
            : base(configuration.GetConnectionString(DatabaseName) ?? string.Empty)
        {
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            var accounts = new List<Account>();
            var roles = new List<Role>();
            using var connection = await OpenConnectionAsync();

            var cmd = new NpgsqlCommand("get_all_user_with_roles", connection as NpgsqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                accounts.Add(new Account()
                {
                    UserId = (int)reader["UserId"],
                    Username = reader["Username"].ToString() ?? string.Empty,
                    Email = reader["Email"].ToString() ?? string.Empty,
                    PasswordHash = reader["PasswordHash"].ToString() ?? string.Empty,
                    CreatedOn = (DateTime)reader["CreatedOn"],
                });

                roles.Add(new Role()
                {
                    Id = (int)reader["RoleId"],
                    Name = reader["RoleName"].ToString() ?? string.Empty,
                    UserId = (int)reader["UserId"],
                });
            }

            return accounts
                .DistinctBy(x => x.UserId)
                .Select(account => 
                {
                    account.Roles = roles.Where(x => x.UserId == account.UserId);
                    return account;
                });
        }

        public async Task<Account?> GetByUsernameAsync(string? username)
        {
            using var connection = await OpenConnectionAsync();
            const string commandText =
                $"SELECT user_id UserId, username Username, password PasswordHash, email Email, created_on CreatedOn, last_login LastLogin FROM {TableName} WHERE username = @username";

            var account = await connection.QueryFirstOrDefaultAsync<Account>(commandText, new { username });
            account?.LoadRoles(connection);

            return account;
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            using var connection = await OpenConnectionAsync();
            const string commandText =
                $"SELECT user_id UserId, username Username, password PasswordHash, email Email, created_on CreatedOn, last_login LastLogin FROM {TableName} WHERE user_id = @id";

            var account = await connection.QueryFirstOrDefaultAsync<Account>(commandText, new { id });
            account.LoadRoles(connection);

            return account;
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = await OpenConnectionAsync();

            var cmd = new NpgsqlCommand("call delete_user(:_user_id)",
            connection as NpgsqlConnection);

            cmd.Parameters.AddWithValue("_user_id", id);
            cmd.CommandType = CommandType.Text;

            await ExecuteNonQuery(cmd);
        }

        public async Task UpdateAsync(int id, Account account)
        {
            using var connection = await OpenConnectionAsync();

            var cmd = new NpgsqlCommand("call update_user(:_user_id, :_username, :_pwd, :_email, :_role_names)",
                connection as NpgsqlConnection);

            cmd.Parameters.AddWithValue("_user_id", id);
            cmd.Parameters.AddWithValue("_username", account.Username);
            cmd.Parameters.AddWithValue("_pwd", account.PasswordHash);
            cmd.Parameters.AddWithValue("_email", account.Email);
            cmd.Parameters.AddWithValue("_role_names", account.Roles.Select(x => x.Name).ToArray());
            cmd.CommandType = CommandType.Text;

            await ExecuteNonQuery(cmd);
        }

        public async Task<Account?> CreateAsync(Account account)
        {
            using var connection = await OpenConnectionAsync();

            var cmd = new NpgsqlCommand("call create_user(:_username, :_pwd, :_email, :_role_names)",
                connection as NpgsqlConnection);

            cmd.Parameters.AddWithValue("_username", account.Username);
            cmd.Parameters.AddWithValue("_pwd", account.PasswordHash);
            cmd.Parameters.AddWithValue("_email", account.Email);
            cmd.Parameters.AddWithValue("_role_names", account.Roles.Select(x => x.Name).ToArray());
            cmd.CommandType = CommandType.Text;

            await ExecuteNonQuery(cmd);
            return await GetByUsernameAsync(account.Username);
        }
    }
}