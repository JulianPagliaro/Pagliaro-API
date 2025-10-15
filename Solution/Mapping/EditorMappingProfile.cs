using AutoMapper;
using GameStore.Application.Dtos.Editor;
using GameStore.Entities;

namespace GameStore.WebAPI.Mapping
{
    public class EditorMappingProfile : Profile
    {
        public EditorMappingProfile()
        {
            CreateMap<Editor, EditorResponseDto>().
                ForMember(dest=>dest.FechaFundacion, ori=>ori.MapFrom(src=>src.FechaFundacion.ToShortDateString()));
           
            CreateMap<EditorRequestDto, Editor>();
        }
    }
}
