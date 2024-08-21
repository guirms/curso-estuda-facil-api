using Domain.Models;

namespace Domain.Objects.Responses.Asset
{
    public record LogInResponse
    {
        public LogInResponse(string token, User user)
        {
            Token = token;
            User = user;
        }

        public string Token { get; set; }
        public User User { get; set; }
    }
}
