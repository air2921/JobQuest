namespace common.DTO;

public class JwtDTO
{
    public string Role { get; set; } = null!;
    public int UserId { get; set; }
    public TimeSpan Expires { get; set; }
}
