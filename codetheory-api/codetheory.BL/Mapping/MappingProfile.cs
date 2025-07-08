using AutoMapper;
using codetheory.BL.DTOs;
using codetheory.DAL.Models;

namespace codetheory.BL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Lesson, LessonDTO>()
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.LessonContents))
                .ReverseMap();

            CreateMap<LessonContent, LessonContentDto>().ReverseMap();
            CreateMap<Answer, AnswerDto>();
            CreateMap<Question, QuestionDto>();
        }
    }
}
