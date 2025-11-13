using AutoMapper;
using GameStore.Application.Dtos.Identity.Roles;
using GameStore.Entities.MicrosoftIdentity;

namespace GameStore.WebAPI.Mapping
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleResponseDto>();
            CreateMap<RoleRequestDto, Role>();
        }
    }
}
