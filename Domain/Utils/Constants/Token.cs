namespace Domain.Utils.Constants
{
    public static class Token
    {
        public const string NameIdentifier = "nameid";
        public const string Authorization = "Authorization";
        public static string JwtKey { get; set; } = string.Empty;
        public static string JwtIssuer { get; set; } = string.Empty;
    }
}
