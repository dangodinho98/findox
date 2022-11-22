namespace Findox.Infra.Data.Repositories.Group
{
    using Dapper;
    using Findox.Domain.Entities;
    using Microsoft.Extensions.Configuration;

    public class GroupRepository : BaseRepository, IGroupRepository
    {
        private const string TableName = "groups";
        public GroupRepository(IConfiguration configuration)
            : base(configuration.GetConnectionString(DatabaseName) ?? string.Empty)
        {
        }

        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            using var connection = await OpenConnectionAsync();
            const string commandText = $"SELECT group_id Id, group_name Name FROM {TableName}";
            var groups = await connection.QueryAsync<Group>(commandText);
            return groups;
        }

        public async Task<Group> GetGroupByIdAsync(int id)
        {
            using var connection = await OpenConnectionAsync();
            const string commandText = $"SELECT group_id Id, group_name Name FROM {TableName} WHERE Id = @id";

            var group = await connection.QueryFirstOrDefaultAsync<Group>(commandText, new { Id = id });
            return group;
        }

        public async Task<int> CreateGroupAsync(Group group)
        {
            using var connection = await OpenConnectionAsync();
            const string commandText = $"INSERT INTO {TableName} (id, Name) VALUES (@id, @name) RETURNING Id";

            return await connection.ExecuteScalarAsync<int>(commandText, new
            {
                id = Guid.NewGuid(),
                name = group.Name,
            });
        }

        public async Task UpdateGroupAsync(int id, Group group)
        {
            using var connection = await OpenConnectionAsync();
            const string commandText = $@"UPDATE {TableName} SET Name = @name WHERE id = @id";

            await connection.ExecuteAsync(commandText, new
            {
                id,
                name = group.Name,
            });
        }

        public async Task DeleteGroupAsync(int id)
        {
            using var connection = await OpenConnectionAsync();
            const string commandText = $"DELETE FROM {TableName} WHERE ID=(@id)";

            await connection.ExecuteAsync(commandText, new { id });
        }
    }
}
