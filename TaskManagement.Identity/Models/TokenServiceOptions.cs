using Microsoft.IdentityModel.Tokens;

namespace TaskManagement.Identity.Models;

public sealed class TokenServiceOptions
{
    public const string Key = "Authentication";

    public required string SecretKey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }

    public SecurityKey GetIssuerSigningKey() =>
        new SymmetricSecurityKey(Convert.FromBase64String(SecretKey));
}