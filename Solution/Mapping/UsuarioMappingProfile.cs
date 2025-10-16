using AutoMapper;
using GameStore.Application.Dtos.Usuario;
using GameStore.Entities;

namespace GameStore.WebAPI.Mapping
{
    public class UsuarioMappingProfile : Profile
    {
        public UsuarioMappingProfile()
        {
            CreateMap<Usuario, UsuarioResponseDto>();
            CreateMap<UsuarioRequestDto, Usuario>();
        }
    }
}
