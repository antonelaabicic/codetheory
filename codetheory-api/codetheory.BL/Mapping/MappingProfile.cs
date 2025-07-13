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
            CreateMap<Answer, AnswerDto>().ReverseMap();
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<UserAnswer, UserAnswerDto>().ReverseMap();
            CreateMap<UserProgress, UserProgressDto>().ReverseMap();

            CreateMap<User, StudentWithProgressDto>()
                .ForMember(dest => dest.Progress, opt => opt.MapFrom(src => src.UserProgresses));
        }
    }
}
