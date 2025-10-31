using AutoMapper;
using GameStore.Application.Dtos.Review;
using GameStore.Entities;

namespace GameStore.WebAPI.Mapping
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<Review, ReviewResponseDto>();

            CreateMap<ReviewRequestDto, Review>();
        }
    }
}
