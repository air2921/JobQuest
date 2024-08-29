using System.Collections.Generic;

namespace domain.SpecDTO;

public class SortCompanyDTO : PaginationDTO
{
    public double? CompanyGrade { get; set; }
    public int? UserId { get; set; }
    public string? CompanyName { get; set; }
    public bool? HasOpenedVacancies { get; set; }
    public IEnumerable<string>? Locations { get; set; }
}
