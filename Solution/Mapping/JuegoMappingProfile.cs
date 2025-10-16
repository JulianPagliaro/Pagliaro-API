using AutoMapper;
using GameStore.Application.Dtos.Editor;
using GameStore.Application.Dtos.Juego;
using GameStore.Entities;

namespace GameStore.WebAPI.Mapping
{
    public class JuegoMappingProfile : Profile
    {
        public JuegoMappingProfile()
        {
            CreateMap<Juego, JuegoResponseDto>().
                ForMember(dest => dest.FechaPublicacion, ori => ori.MapFrom(src => src.FechaPublicacion.ToShortDateString()));

            CreateMap<JuegoRequestDto, Juego>();
        }


    }
}
