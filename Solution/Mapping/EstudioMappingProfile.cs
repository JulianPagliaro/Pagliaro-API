using AutoMapper;
using GameStore.Application.Dtos.Estudio;
using GameStore.Entities;

namespace GameStore.WebAPI.Mapping
{
    public class EstudioMappingProfile : Profile
    {
        public EstudioMappingProfile()
        {
            CreateMap<Estudio, EstudioResponseDto>().
                ForMember(dest => dest.FechaFundacion, ori => ori.MapFrom(src => src.FechaFundacion.ToShortDateString()));
        
            CreateMap<EstudioRequestDto, Estudio>();
        }
    }
}

