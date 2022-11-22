namespace Findox.Application.Services.Group
{
    using Findox.Application.Dto.Group;
    using Findox.Domain.Entities;

    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAsync();
        Task<Group> GetByIdAsync(int id);
        Task<int> CreateAsync(GroupDto groupDto);
        Task UpdateAsync(int id, GroupDto groupDto);
        Task DeleteAsync(int id);
    }
}
