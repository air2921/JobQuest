using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace domain.SpecDTO;

public class SortVacancyDTO : PaginationDTO
{
    public int? CompanyId { get; set; }
    public bool? IsOpened { get; set; }
    public IEnumerable<string>? Locations { get; set; }
    public int? MinSalary { get; set; }
    public int? MaxSalary { get; set; }
    public int? MinExperience { get; set; }
    public int? MaxExperience { get; set; }
    public IEnumerable<int>? EducationLevels { get; set; }
    public IEnumerable<int>? Employments { get; set; }
    public IEnumerable<int>? WorkSchedules { get; set; }
    public string? Name { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder(512);

        builder.Append(CompanyId.ToString() ?? "null");
        builder.Append('-');
        builder.Append(IsOpened.ToString() ?? "null");
        builder.Append('-');

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

        builder.Append(MinSalary.ToString() ?? "null");
        builder.Append('-');
        builder.Append(MaxSalary.ToString() ?? "null");
        builder.Append('-');
        builder.Append(MinExperience.ToString() ?? "null");
        builder.Append('-');
        builder.Append(MaxExperience.ToString() ?? "null");
        builder.Append('-');

        if (EducationLevels is not null && EducationLevels.Any())
        {
            foreach (var level in EducationLevels)
            {
                builder.Append(level.ToString() ?? "null");
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

        builder.Append(Name ?? "null");
        builder.Append('-');

        builder.Append(Skip);
        builder.Append('-');
        builder.Append(Total);
        builder.Append('-');
        builder.Append(ByDesc);

        return builder.ToString();
    }
}
