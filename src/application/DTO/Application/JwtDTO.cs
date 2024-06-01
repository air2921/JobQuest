using System;

namespace application.DTO.Application
{
    public class JwtDTO
    {
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int UserId { get; set; }
        public TimeSpan Expires { get; set; }
    }
}
