using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.SpecDTO;

public class SortLanguageDTO : PaginationDTO
{
    public string[]? Languages { get; set; }
    public int[]? Levels { get; set; }
}
