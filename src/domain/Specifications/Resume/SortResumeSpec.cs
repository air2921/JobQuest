using Ardalis.Specification;
using domain.Enums;
using domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace domain.Specifications.Resume;

public class SortResumeSpec : SortCollectionSpec<ResumeModel>
{
    public SortResumeSpec(int skip, int count, bool byDesc)
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        if (UserId.HasValue)
            Query.Where(x => x.UserId.Equals(UserId));

        if (MinSalary.HasValue)
            Query.Where(x => x.MinSalary >= MinSalary);

        if (MaxSalary.HasValue)
            Query.Where(x => x.MaxSalary <= MaxSalary);

        if (Statuses is not null && Statuses.Any())
            Query.Where(x => Statuses.Contains(x.Status));

        if (Employments is not null && Employments.Any())
            Query.Where(x => Employments.Contains(x.Employment));

        if (WorkSchedules is not null && WorkSchedules.Any())
            Query.Where(x => WorkSchedules.Contains(x.WorkSchedule));

        if (Locations is not null && Locations.Any())
            Query.Where(x => Locations.Contains(x.Location));

        if (Citizenships is not null && Citizenships.Any())
            Query.Where(x => Citizenships.Contains(x.Citizenship));

        if (WorkPermits is not null && WorkPermits.Any())
            Query.Where(x => WorkPermits.Contains(x.WorkPermit));

        if (SpecialityNames is not null && SpecialityNames.Any())
            Query.Where(x => SpecialityNames.Contains(x.SpecialityName));

        if (Skills is not null && Skills.Any())
            Query.Where(x => x.Skills != null && x.Skills.Length >= Skills.Count() && Skills.All(skill => x.Skills.Contains(skill)));

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
                Query.Where(x => x.Experiences.Sum(ex => ex.ExperienceCountInMounts) >= MinExp.GetValueOrDefault(0) &&
                    x.Experiences.Sum(ex => ex.ExperienceCountInMounts) <= MaxExp.GetValueOrDefault(int.MaxValue));
            }

            if (StillWorks.HasValue)
            {
                if (StillWorks.Value)
                    Query.Where(x => x.Experiences.Any(ex => ex.IsPresentTime));
                else
                    Query.Where(x => x.Experiences.All(ex => !ex.IsPresentTime));
            }

            if (HasDuties.HasValue)
                Query.Where(x => x.Experiences.All(ex => !string.IsNullOrWhiteSpace(ex.Duties)));
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

    public IEnumerable<int>? Statuses { get; set; }
    public IEnumerable<int>? Employments { get; set; }
    public IEnumerable<int>? WorkSchedules { get; set; }
    public IEnumerable<string>? Locations { get; set; }
    public IEnumerable<string>? Citizenships { get; set; }
    public IEnumerable<string>? WorkPermits { get; set; }

    public IEnumerable<string>? SpecialityNames { get; set; }
    public IEnumerable<string>? Skills { get; set; }
    public Dictionary<Enums.Language, LanguageLevel>? Languages { get; set; }

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
