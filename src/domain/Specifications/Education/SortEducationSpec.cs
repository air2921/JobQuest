using Ardalis.Specification;
using domain.SpecDTO;
using domain.Models;
using System.Linq;
using System;

namespace domain.Specifications.Education;

public class SortEducationSpec : SortCollectionSpec<EducationModel>
{
    public SortEducationSpec(int skip, int count, bool byDesc) 
        : base(skip, count, byDesc, x => x.Level)
    {
        if (DTO is null)
        {
            Initialize();
            return;
        }

        if (DTO.ResumeId.HasValue)
            Query.Where(x => x.ResumeId.Equals(DTO.ResumeId));

        if (DTO.Levels is not null && DTO.Levels.Any())
            Query.Where(x => DTO.Levels.Contains(x.Level));

        if (DTO.Institutions is not null && DTO.Institutions.Any())
            Query.Where(x => DTO.Institutions.Contains(x.Institution));

        if (DTO.Specialties is not null && DTO.Specialties.Any())
            Query.Where(x => DTO.Specialties.Contains(x.Specialty));

        if (DTO.StillStyding.HasValue)
            Query.Where(x => x.IsPresentTime.Equals(DTO.StillStyding));

        Initialize();
    }

    public SortEducationDTO? DTO { get; set; }
}
