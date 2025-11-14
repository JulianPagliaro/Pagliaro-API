using AutoMapper;
using GameStore.Application.Dtos.Pais;
using GameStore.Entities;

namespace GameStore.WebAPI.Mapping
{
    public class PaisMappingProfile : Profile
    {
        public PaisMappingProfile()
        {
            CreateMap<Pais, PaisResponseDto>();

            CreateMap<PaisRequestDto, Pais>();
        }
    }
}
