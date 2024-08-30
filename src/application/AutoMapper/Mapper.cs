using AutoMapper;
using common.DTO.ModelDTO;
using domain.Models;

namespace application.AutoMapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<ResponseDTO, ResponseModel>();
        CreateMap<CompanyDTO, CompanyModel>();
        CreateMap<ResumeDTO, ResponseModel>();
        CreateMap<ExperienceDTO, ExperienceModel>();
        CreateMap<EducationDTO, EducationModel>();
        CreateMap<VacancyDTO, VacancyModel>();
        CreateMap<ReviewDTO, ReviewModel>();
    }
}
