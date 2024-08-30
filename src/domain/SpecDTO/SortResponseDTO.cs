using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.SpecDTO;

public class SortResponseDTO : PaginationDTO
{
    public int? Status { get; set; }
    public int? Reason { get; set; }
    public bool? HasDescription { get; set; }
}
