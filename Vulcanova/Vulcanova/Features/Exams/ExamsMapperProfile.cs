using AutoMapper;
using Vulcanova.Uonet.Api.Exams;

namespace Vulcanova.Features.Exams
{
    public class ExamsMapperProfile : Profile
    {
        public ExamsMapperProfile()
        {
            CreateMap<ExamPayload, Exam>();
        }
    }
}