namespace Domain.Objects.Dto_s.User
{
    public record UserAuthInfoDto
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string Salt { get; set; }
    }
}
