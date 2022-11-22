namespace Findox.Infra.Data.Repositories.Group
{
    using Findox.Domain.Entities;

    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetGroupsAsync();
        Task<Group> GetGroupByIdAsync(int id);
        Task<int> CreateGroupAsync(Group group);
        Task UpdateGroupAsync(int id, Group group);
        Task DeleteGroupAsync(int id);
    }
}
