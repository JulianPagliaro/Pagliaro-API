using AutoMapper;
using GameStore.Application.Dtos.Pais;
using GameStore.Application.Dtos.Review;
using GameStore.Entities;

namespace GameStore.WebAPI.Mapping
{
    public class PaisMappingProfile : Profile
    {
        public PaisMappingProfile()
        {
            CreateMap<Review, PaisResponseDto>();

            CreateMap<PaisRequestDto, Review>();
        }
    }
}
