using AutoMapper;
using GameStore.Application.Dtos.Review;
using GameStore.Entities;

namespace GameStore.WebAPI.Mapping
{
    public class ReviewMappingDto : Profile
    {
        public ReviewMappingDto()
        {
            CreateMap<Review, ReviewResponseDto>();

            CreateMap<ReviewRequestDto, Review>();
        }
    }
}
