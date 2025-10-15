using AutoMapper;
using GameStore.Application.Dtos.Genero;
using GameStore.Entities;

namespace GameStore.WebAPI.Mapping
{
    public class GeneroMappingProfile : Profile
    {
        public GeneroMappingProfile()
        {
            CreateMap<Genero, GeneroResponseDto>();
            CreateMap<GeneroRequestDto, Genero>();
        }
    }
}
