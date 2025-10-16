using AutoMapper;
using GameStore.Application.Dtos.Plataforma;
using GameStore.Entities;

namespace GameStore.WebAPI.Mapping
{
    public class PlataformaMappingProfile :Profile
    {
        public PlataformaMappingProfile()
        {
            CreateMap<Plataforma, PlataformaResponseDto>().
                ForMember(dest=>dest.FechaLanzamiento, ori=>ori.MapFrom(src=>src.FechaLanzamiento.ToShortDateString()));

            CreateMap<PlataformaRequestDto, Plataforma>();
        }
    }
}
