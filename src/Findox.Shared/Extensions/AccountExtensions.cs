using System.Data;
using Dapper;
using Findox.Domain.Entities;

namespace Findox.Shared.Extensions;

public static class AccountExtensions
{
    public static void LoadRoles(this Account account, IDbConnection connection)
    {
        var role_ids =
            connection.Query<int>("select role_id from account_roles where user_id = @id", new { id = account.UserId }).ToArray();

        var roles = connection.Query<Role>(
            "select role_id Id, role_name Name from roles where role_id = ANY (@ids)", new { ids = role_ids });

        account.Roles = roles;
    }
}