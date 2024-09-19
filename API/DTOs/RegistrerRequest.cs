

namespace API.DTOs
{
    public class RegistrerRequest
    {
        public required string UserName{ get; set; } = "";
        public required string Password { get; set; } = "";
    }
}