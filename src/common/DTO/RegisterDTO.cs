namespace common.DTO;

public class RegisterDTO
{
    private string _email = null!;

    public string Email
    {
        get => _email;
        set => _email = value.ToLowerInvariant();
    }
    public string Password { get; set; } = null!;
    public bool AsEmployer { get; set; } = false;
}
