using AutoMapper;
using common.DTO.ModelDTO;
using common.DTO.ModelDTO.Chat;
using domain.Models;
using domain.Models.Chat;

namespace application.AutoMapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<ChatDTO, ChatModel>();
        CreateMap<MessageDTO, MessageModel>();
        CreateMap<LanguageDTO, LanguageModel>();
        CreateMap<ResponseDTO, ResponseModel>();
        CreateMap<CompanyDTO, CompanyModel>();
        CreateMap<ResumeDTO, ResponseModel>();
        CreateMap<ExperienceDTO, ExperienceModel>();
        CreateMap<EducationDTO, EducationModel>();
        CreateMap<VacancyDTO, VacancyModel>();
        CreateMap<ReviewDTO, ReviewModel>();
    }
}
