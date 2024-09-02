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
        CreateMap<ChatDTO, ChatModel>()
            .ForMember(x => x.CandidateUser, options => options.Ignore())
            .ForMember(x => x.EmployerUser, options => options.Ignore())
            .ForMember(x => x.Messages, options => options.Ignore());

        CreateMap<MessageDTO, MessageModel>()
            .ForMember(x => x.Candidate, options => options.Ignore())
            .ForMember(x => x.Employer, options => options.Ignore())
            .ForMember(x => x.Chat, options => options.Ignore());

        CreateMap<LanguageDTO, LanguageModel>()
            .ForMember(x => x.User, options => options.Ignore());

        CreateMap<ResponseDTO, ResponseModel>()
            .ForMember(x => x.Resume, options => options.Ignore())
            .ForMember(x => x.Vacancy, options => options.Ignore());

        CreateMap<CompanyDTO, CompanyModel>()
             .ForMember(x => x.User, options => options.Ignore())
             .ForMember(x => x.EmployerChats, options => options.Ignore())
             .ForMember(x => x.Reviews, options => options.Ignore())
             .ForMember(x => x.SentMessagesAsEmployer, options => options.Ignore())
             .ForMember(x => x.EmployerChats, options => options.Ignore());

        CreateMap<ResumeDTO, ResponseModel>()
              .ForMember(x => x.Resume, options => options.Ignore())
              .ForMember(x => x.Vacancy, options => options.Ignore());

        CreateMap<ExperienceDTO, ExperienceModel>()
              .ForMember(x => x.Resume, options => options.Ignore());

        CreateMap<EducationDTO, EducationModel>()
            .ForMember(x => x.Resume, options => options.Ignore());

        CreateMap<VacancyDTO, VacancyModel>()
              .ForMember(x => x.Company, options => options.Ignore())
              .ForMember(x => x.Favorites, options => options.Ignore())
              .ForMember(x => x.Responses, options => options.Ignore());

        CreateMap<ReviewDTO, ReviewModel>()
              .ForMember(x => x.Company, options => options.Ignore())
              .ForMember(x => x.User, options => options.Ignore());
    }
} 
