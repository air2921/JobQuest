namespace application.DTO.Api
{
    public class RegisterDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool AsCandidate { get; set; }
    }
}
