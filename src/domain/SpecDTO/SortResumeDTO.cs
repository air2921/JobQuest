using domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace domain.SpecDTO;

public class SortResumeDTO : PaginationDTO
{
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
    public Dictionary<Language, LanguageLevel>? Languages { get; set; }

    public int? MinExp { get; set; }
    public int? MaxExp { get; set; }
    public bool? StillWorks { get; set; }
    public bool? HasDuties { get; set; }

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

    public override string ToString()
    {
        var builder = new StringBuilder(1024);

        builder.Append(UserId.ToString() ?? "null");
        builder.Append('-');

        builder.Append(MinSalary.ToString() ?? "null");
        builder.Append('-');

        builder.Append(MaxSalary.ToString() ?? "null");
        builder.Append('-');

        if (Statuses is not null && Statuses.Any())
        {
            foreach (var status in Statuses)
            {
                builder.Append(status.ToString() ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (Employments is not null && Employments.Any())
        {
            foreach (var employment in Employments)
            {
                builder.Append(employment.ToString() ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (WorkSchedules is not null && WorkSchedules.Any())
        {
            foreach (var schedule in WorkSchedules)
            {
                builder.Append(schedule.ToString() ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (Locations is not null && Locations.Any())
        {
            foreach (var location in Locations)
            {
                builder.Append(location ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (Citizenships is not null && Citizenships.Any())
        {
            foreach (var citizenship in Citizenships)
            {
                builder.Append(citizenship ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (WorkPermits is not null && WorkPermits.Any())
        {
            foreach (var permit in WorkPermits)
            {
                builder.Append(permit ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (SpecialityNames is not null && SpecialityNames.Any())
        {
            foreach (var name in SpecialityNames)
            {
                builder.Append(name ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (Skills is not null && Skills.Any())
        {
            foreach (var skill in Skills)
            {
                builder.Append(skill ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (Languages is not null && Languages.Any())
        {
            foreach (var kvp in Languages)
            {
                builder.Append(kvp.Key.ToString() ?? "null");
                builder.Append(':');
                builder.Append(kvp.Value.ToString() ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        builder.Append(MinExp.ToString() ?? "null");
        builder.Append('-');

        builder.Append(MaxExp.ToString() ?? "null");
        builder.Append('-');

        builder.Append(StillWorks.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasDuties.ToString() ?? "null");
        builder.Append('-');

        builder.Append(MinAge.ToString() ?? "null");
        builder.Append('-');

        builder.Append(MaxAge.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasAbout.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasPhoto.ToString() ?? "null");
        builder.Append('-');

        builder.Append(IsMale.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasPhoneNumber.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasEmail.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasTelegram.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasGithub.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasLinkedIn.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasWebSite.ToString() ?? "null");
        builder.Append('-');

        builder.Append(Skip);
        builder.Append('-');
        builder.Append(Total);
        builder.Append('-');
        builder.Append(ByDesc);

        return builder.ToString();
    }
}
