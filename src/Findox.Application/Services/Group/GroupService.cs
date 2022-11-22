namespace Findox.Application.Services.Group
{
    using AutoMapper;
    using Findox.Application.Dto.Group;
    using Findox.Infra.Data.Repositories.Group;
    using Group = Domain.Entities.Group;

    public class GroupService: IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<Group>> GetAsync()
        {
            return await _groupRepository.GetGroupsAsync();
        }

        public async Task<Group> GetByIdAsync(int id)
        {
            return await _groupRepository.GetGroupByIdAsync(id);
        }

        public async Task<int> CreateAsync(GroupDto groupDto)
        {
            var group = _mapper.Map<Group>(groupDto);
            return await _groupRepository.CreateGroupAsync(group);
        }

        public async Task UpdateAsync(int id, GroupDto groupDto)
        {
            var group = _mapper.Map<Group>(groupDto);
            await _groupRepository.UpdateGroupAsync(id, group);
        }

        public async Task DeleteAsync(int id)
        {
            await _groupRepository.DeleteGroupAsync(id);
        }
    }
}
