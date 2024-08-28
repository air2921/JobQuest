namespace domain.SpecDTO;

public class PaginationDTO
{
    public int Skip { get; set; } = 0;
    public int Total { get; set; } = 50;
    public bool ByDesc { get; set; } = true;
}
