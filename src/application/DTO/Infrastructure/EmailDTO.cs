namespace application.DTO.Infrastructure
{
    public class EmailDTO
    {
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
