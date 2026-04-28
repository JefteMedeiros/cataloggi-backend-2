namespace cataloggi_backend_2.Options;

public class AuthOptions
{
    public const string SectionName = "Auth";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int TokenLifetimeHours { get; set; } = 24;
}
