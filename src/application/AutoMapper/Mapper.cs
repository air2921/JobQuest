using AutoMapper;
using common.DTO.ModelDTO;
using domain.Models;

namespace application.AutoMapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<CompanyDTO, CompanyModel>();
        CreateMap<ResumeDTO, ResponseModel>();
        CreateMap<ExperienceDTO, ExperienceModel>();
        CreateMap<EducationDTO, EducationModel>();
        CreateMap<VacancyDTO, VacancyModel>();
    }
}
