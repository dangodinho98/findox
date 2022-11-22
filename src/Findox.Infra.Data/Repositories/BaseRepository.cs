namespace Findox.Infra.Data.Repositories
{
    using Npgsql;
    using System.Data;

    public class BaseRepository
    {
        protected const string DatabaseName = "FindoxDb";
        private readonly string _connectionString;

        protected BaseRepository(string connectionString)
        {
            _connectionString = !string.IsNullOrEmpty(connectionString)
                ? connectionString
                : throw new ArgumentNullException(nameof(connectionString));
        }

        protected async Task<IDbConnection> OpenConnectionAsync()
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            return conn;
        }

        protected static async Task ExecuteNonQuery(NpgsqlCommand cmd)
        {
            foreach (NpgsqlParameter parameter in cmd.Parameters)
            {
                if (!string.IsNullOrEmpty(parameter.Value?.ToString())) continue;
                parameter.IsNullable = true;
                parameter.Value = DBNull.Value;
            }

            await cmd.ExecuteNonQueryAsync();
        }
    }
}