using TaskManagement.Identity.Models;
using TaskManagement.Identity.Services.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using TaskManagement.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Identity.Services.Implementations;

public sealed class JwtTokenService : ITokenService
{
    private readonly TokenServiceOptions _options;
    private readonly ITokenRepository _tokenRepository;

    public JwtTokenService(IOptions<TokenServiceOptions> options, ITokenRepository tokenRepository)
    {
        _options = options.Value;
        _tokenRepository = tokenRepository;
    }

    public async Task <string> GenerateAccessTokenForAsync(ApplicationUser user,UserManager<ApplicationUser> userManager)
    {
        var signInCredentials = new SigningCredentials(_options.GetIssuerSigningKey(), SecurityAlgorithms.HmacSha256);
        var userRoles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new ("sub", user.Id),
            new ("preferred_username", user.UserName),
            new ("first_name", user.FirstName),
            new ("last_name", user.LastName),    
            
        };

        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(10),
            signInCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    public async Task<string> GenerateRefreshTokenAsync(ApplicationUser user)
    {
        var refreshToken = RefreshToken.CreateNew(user.Id);
        await _tokenRepository.AddRefreshTokenAsync(refreshToken);
        return refreshToken.Value;
    }

    public async Task<string> RefreshTokenAsync(string token)
    {
        var refreshToken = await _tokenRepository.GetRefreshTokenAsync(token);
        if (refreshToken is null)
        {
            throw new AuthenticationException("Invalid refresh token");
        }

        if (refreshToken.ExpiresAt <= DateTime.Now)
        {
            throw new AuthenticationException("Refresh token has expired");
        }

        refreshToken.Refresh();
        await _tokenRepository.UpdateAsync(refreshToken);

        return refreshToken.Value;
    }
}