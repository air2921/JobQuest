using Ardalis.Specification;
using domain.Enums;
using domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace domain.Specifications.Resume;

public class SortResumeSpec : SortCollectionSpec<ResumeModel>
{
    public SortResumeSpec(int skip, int count, bool byDesc, IEnumerable<ExperienceModel> experiences)
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        if (UserId.HasValue)
            Query.Where(x => x.UserId.Equals(UserId));

        if (MinSalary.HasValue)
            Query.Where(x => x.MinSalary >= MinSalary);

        if (MaxSalary.HasValue)
            Query.Where(x => x.MaxSalary <= MaxSalary);

        if (Statuses is not null && Statuses.Length > 0)
            Query.Where(x => Statuses.Contains(x.Status));

        if (Employments is not null && Employments.Length > 0)
            Query.Where(x => Employments.Contains(x.Employment));

        if (WorkSchedules is not null && WorkSchedules.Length > 0)
            Query.Where(x => WorkSchedules.Contains(x.WorkSchedule));

        if (Locations is not null && Locations.Length > 0)
            Query.Where(x => Locations.Contains(x.Location));

        if (Citizenships is not null && Citizenships.Length > 0)
            Query.Where(x => Citizenships.Contains(x.Citizenship));

        if (WorkPermits is not null && WorkPermits.Length > 0)
            Query.Where(x => WorkPermits.Contains(x.WorkPermit));

        if (SpecializationNames is not null && SpecializationNames.Length > 0)
            Query.Where(x => SpecializationNames.Contains(x.SpecializationName));

        if (Skills is not null && Skills.Length > 0)
            Query.Where(x => x.Skills != null && x.Skills.Length >= Skills.Length && Skills.All(skill => x.Skills.Contains(skill)));

        if (Languages is not null && Languages.Count > 0)
        {
            Query.Where(resume =>
                resume.Languages != null &&
                resume.Languages.Count >= Languages.Count &&
                Languages.All(lang =>
                    resume.Languages.Any(langModel =>
                        langModel.LanguageName == lang.Key.ToString() &&
                        langModel.Level >= (int)lang.Value)));
        }

        if (MinExp.HasValue || MaxExp.HasValue || StillWorks.HasValue || HasDuties.HasValue)
        {
            Query.Where(x => x.Experiences != null && x.Experiences.Count > 0);

            if (MinExp.HasValue || MaxExp.HasValue)
            {
                Query.Where(x => experiences.Sum(ex => ex.ExperienceCountInMounts) >= MinExp.GetValueOrDefault(0) &&
                    experiences.Sum(ex => ex.ExperienceCountInMounts) <= MaxExp.GetValueOrDefault(int.MaxValue));
            }

            if (StillWorks.HasValue)
            {
                if (StillWorks.Value)
                    Query.Where(x => experiences.Any(ex => ex.IsPresentTime));
                else
                    Query.Where(x => experiences.All(ex => !ex.IsPresentTime));
            }

            if (HasDuties.HasValue)
                Query.Where(x => experiences.All(ex => !string.IsNullOrWhiteSpace(ex.Duties)));
        }

        if (MinAge.HasValue)
            Query.Where(x => x.Age >= MinAge);

        if (MaxAge.HasValue)
            Query.Where(x => x.Age <= MaxAge);

        if (IsMale.HasValue)
            Query.Where(x => x.IsMale.Equals(IsMale));

        if (HasAbout.HasValue)
            Query.Where(x => HasAbout.Value
                ? !string.IsNullOrWhiteSpace(x.About)
                : string.IsNullOrWhiteSpace(x.About));

        if (HasPhoto.HasValue)
            Query.Where(x => HasPhoto.Value
                ? !string.IsNullOrWhiteSpace(x.ImageKey)
                : string.IsNullOrWhiteSpace(x.ImageKey));

        if (HasPhoneNumber.HasValue)
            Query.Where(x => HasPhoneNumber.Value
                ? !string.IsNullOrWhiteSpace(x.PhoneNumber)
                : string.IsNullOrWhiteSpace(x.PhoneNumber));

        if (HasEmail.HasValue)
            Query.Where(x => HasEmail.Value
                ? !string.IsNullOrWhiteSpace(x.Email)
                : string.IsNullOrWhiteSpace(x.Email));

        if (HasTelegram.HasValue)
            Query.Where(x => HasTelegram.Value
                ? !string.IsNullOrWhiteSpace(x.Telegram)
                : string.IsNullOrWhiteSpace(x.Telegram));

        if (HasGithub.HasValue)
            Query.Where(x => HasGithub.Value
                ? !string.IsNullOrWhiteSpace(x.Github)
                : string.IsNullOrWhiteSpace(x.Github));

        if (HasLinkedIn.HasValue)
            Query.Where(x => HasLinkedIn.Value
                ? !string.IsNullOrWhiteSpace(x.LinkedIn)
                : string.IsNullOrWhiteSpace(x.LinkedIn));

        if (HasWebSite.HasValue)
            Query.Where(x => HasWebSite.Value
                ? !string.IsNullOrWhiteSpace(x.WebSite)
                : string.IsNullOrWhiteSpace(x.WebSite));

        Initialize();
    }

    public int? UserId { get; set; }

    public int? MinSalary { get; set; }
    public int? MaxSalary { get; set; }

    public int[]? Statuses { get; set; }
    public int[]? Employments { get; set; }
    public int[]? WorkSchedules { get; set; }
    public string[]? Locations { get; set; }
    public string[]? Citizenships { get; set; }
    public string[]? WorkPermits { get; set; }

    public string[]? SpecializationNames { get; set; }
    public string[]? Skills { get; set; }
    public Dictionary<Language, LanguageLevel>? Languages { get; set; }

    public int? MinExp { get; set; }
    public int? MaxExp { get; set; }
    public bool? StillWorks { get; set; }
    public bool? HasDuties  { get; set; }

    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }

    public bool? HasAbout { get; set; }
    public bool? HasPhoto { get; set; }
    public bool? IsMale { get; set; }

    public bool? HasPhoneNumber { get; set; }
    public bool? HasEmail { get; set; }
    public bool? HasTelegram { get; set; }
    public bool? HasGithub { get; set; }
    public bool? HasLinkedIn { get; set; }
    public bool? HasWebSite { get; set; }
}
