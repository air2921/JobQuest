using Ardalis.Specification;
using domain.Attributes;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace domain.Specifications.Education;

public class SortEducationSpec : SortCollectionSpec<EducationModel>
{
    public SortEducationSpec(int skip, int count, bool byDesc) 
        : base(skip, count, byDesc, x => x.Level)
    {
        if (ResumeId.HasValue)
            Query.Where(x => x.ResumeId.Equals(ResumeId));

        if (Levels is not null && Levels.Any())
            Query.Where(x => Levels.Contains(x.Level));

        if (Institutions is not null && Institutions.Any())
            Query.Where(x => Institutions.Contains(x.Institution));

        if (Specialties is not null && Specialties.Any())
            Query.Where(x => Specialties.Contains(x.Specialty));

        if (StillStyding.HasValue)
            Query.Where(x => x.IsPresentTime.Equals(StillStyding));

        Initialize();
    }

    public int? ResumeId { get; set; }
    public IEnumerable<int>? Levels { get; set; }
    public IEnumerable<string>? Institutions { get; set; }
    public IEnumerable<string>? Specialties { get; set; }
    public bool? StillStyding { get; set; }
}
