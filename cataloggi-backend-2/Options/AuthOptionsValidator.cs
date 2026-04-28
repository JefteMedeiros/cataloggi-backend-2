using System.Text;

namespace cataloggi_backend_2.Options;

public static class AuthOptionsValidator
{
    public static void Validate(AuthOptions authOptions)
    {
        if (string.IsNullOrWhiteSpace(authOptions.Issuer)
            || string.IsNullOrWhiteSpace(authOptions.Audience)
            || string.IsNullOrWhiteSpace(authOptions.SigningKey)
            || string.IsNullOrWhiteSpace(authOptions.Username)
            || string.IsNullOrWhiteSpace(authOptions.Password))
            throw new InvalidOperationException("Auth configuration is incomplete.");

        if (Encoding.UTF8.GetByteCount(authOptions.SigningKey) < 32)
            throw new InvalidOperationException("Auth signing key must be at least 32 bytes.");
    }
}
