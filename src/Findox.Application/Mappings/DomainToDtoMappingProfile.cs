namespace Findox.Application.Mappings
{
    using AutoMapper;
    using Findox.Application.Dto.Group;
    using Findox.Domain.Entities;

    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            CreateMap<Group, GroupDto>().ReverseMap();
        }
    }
}
