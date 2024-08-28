using Ardalis.Specification;
using domain.SpecDTO;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Resume;

public class SortResumeSpec : SortCollectionSpec<ResumeModel>
{
    public SortResumeSpec(int skip, int count, bool byDesc)
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        if (DTO is null)
        {
            Initialize();
            return;
        }

        if (DTO.UserId.HasValue)
            Query.Where(x => x.UserId.Equals(DTO.UserId));

        if (DTO.MinSalary.HasValue)
            Query.Where(x => x.MinSalary >= DTO.MinSalary);

        if (DTO.MaxSalary.HasValue)
            Query.Where(x => x.MaxSalary <= DTO.MaxSalary);

        if (DTO.Statuses is not null && DTO.Statuses.Any())
            Query.Where(x => DTO.Statuses.Contains(x.Status));

        if (DTO.Employments is not null && DTO.Employments.Any())
            Query.Where(x => DTO.Employments.Contains(x.Employment));

        if (DTO.WorkSchedules is not null && DTO.WorkSchedules.Any())
            Query.Where(x => DTO.WorkSchedules.Contains(x.WorkSchedule));

        if (DTO.Locations is not null && DTO.Locations.Any())
            Query.Where(x => DTO.Locations.Contains(x.Location));

        if (DTO.Citizenships is not null && DTO.Citizenships.Any())
            Query.Where(x => DTO.Citizenships.Contains(x.Citizenship));

        if (DTO.WorkPermits is not null && DTO.WorkPermits.Any())
            Query.Where(x => DTO.WorkPermits.Contains(x.WorkPermit));

        if (DTO.SpecialityNames is not null && DTO.SpecialityNames.Any())
            Query.Where(x => DTO.SpecialityNames.Contains(x.SpecialityName));

        if (DTO.Skills is not null && DTO.Skills.Any())
            Query.Where(x => x.Skills != null && x.Skills.Length >= DTO.Skills.Count()
            && DTO.Skills.All(skill => x.Skills.Contains(skill)));

        if (DTO.Languages is not null && DTO.Languages.Count > 0)
        {
            Query.Where(resume =>
                resume.LanguageResumes != null &&
                resume.LanguageResumes.Count >= DTO.Languages.Count &&
                DTO.Languages.All(lang =>
                    resume.LanguageResumes.Any(languageResume =>
                        languageResume.Language != null &&
                        languageResume.Language.LanguageName == lang.Key.ToString() &&
                        languageResume.Level >= (int)lang.Value)));
        }

        if (DTO.MinExp.HasValue || DTO.MaxExp.HasValue || DTO.StillWorks.HasValue || DTO.HasDuties.HasValue)
        {
            Query.Where(x => x.Experiences != null && x.Experiences.Count > 0);

            if (DTO.MinExp.HasValue || DTO.MaxExp.HasValue)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                Query.Where(x => x.Experiences.Sum(ex => ex.ExperienceCountInMounts) >= DTO.MinExp.GetValueOrDefault(0) &&
                    x.Experiences.Sum(ex => ex.ExperienceCountInMounts) <= DTO.MaxExp.GetValueOrDefault(int.MaxValue));
#pragma warning restore CS8604 // Possible null reference argument.
            }

            if (DTO.StillWorks.HasValue)
            {
                if (DTO.StillWorks.Value)
#pragma warning disable CS8604 // Possible null reference argument.
                    Query.Where(x => x.Experiences.Any(ex => ex.IsPresentTime));
#pragma warning restore CS8604 // Possible null reference argument.
                else
#pragma warning disable CS8604 // Possible null reference argument.
                    Query.Where(x => x.Experiences.All(ex => !ex.IsPresentTime));
#pragma warning restore CS8604 // Possible null reference argument.
            }

            if (DTO.HasDuties.HasValue)
#pragma warning disable CS8604 // Possible null reference argument.
                Query.Where(x => x.Experiences.All(ex => !string.IsNullOrWhiteSpace(ex.Duties)));
#pragma warning restore CS8604 // Possible null reference argument.
        }

        if (DTO.MinAge.HasValue)
            Query.Where(x => x.Age >= DTO.MinAge);

        if (DTO.MaxAge.HasValue)
            Query.Where(x => x.Age <= DTO.MaxAge);

        if (DTO.IsMale.HasValue)
            Query.Where(x => x.IsMale.Equals(DTO.IsMale));

        if (DTO.HasAbout.HasValue)
            Query.Where(x => DTO.HasAbout.Value
                ? !string.IsNullOrWhiteSpace(x.About)
                : string.IsNullOrWhiteSpace(x.About));

        if (DTO.HasPhoto.HasValue)
            Query.Where(x => DTO.HasPhoto.Value
                ? !string.IsNullOrWhiteSpace(x.ImageKey)
                : string.IsNullOrWhiteSpace(x.ImageKey));

        if (DTO.HasPhoneNumber.HasValue)
            Query.Where(x => DTO.HasPhoneNumber.Value
                ? !string.IsNullOrWhiteSpace(x.PhoneNumber)
                : string.IsNullOrWhiteSpace(x.PhoneNumber));

        if (DTO.HasEmail.HasValue)
            Query.Where(x => DTO.HasEmail.Value
                ? !string.IsNullOrWhiteSpace(x.Email)
                : string.IsNullOrWhiteSpace(x.Email));

        if (DTO.HasTelegram.HasValue)
            Query.Where(x => DTO.HasTelegram.Value
                ? !string.IsNullOrWhiteSpace(x.Telegram)
                : string.IsNullOrWhiteSpace(x.Telegram));

        if (DTO.HasGithub.HasValue)
            Query.Where(x => DTO.HasGithub.Value
                ? !string.IsNullOrWhiteSpace(x.Github)
                : string.IsNullOrWhiteSpace(x.Github));

        if (DTO.HasLinkedIn.HasValue)
            Query.Where(x => DTO.HasLinkedIn.Value
                ? !string.IsNullOrWhiteSpace(x.LinkedIn)
                : string.IsNullOrWhiteSpace(x.LinkedIn));

        if (DTO.HasWebSite.HasValue)
            Query.Where(x => DTO.HasWebSite.Value
                ? !string.IsNullOrWhiteSpace(x.WebSite)
                : string.IsNullOrWhiteSpace(x.WebSite));

        Initialize();
    }

    public SortResumeDTO? DTO { get; set; }
}
