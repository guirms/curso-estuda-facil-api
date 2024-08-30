namespace Domain.Objects.Responses.Asset
{
    public record LogInResponse
    {
        public required string AuthToken { get; set; }
    }
}
