using FluxoDeCaixa.Lancamento.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FluxoDeCaixa.Lancamento.Common.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly SecurityConfig _securitySettions;

    public JwtTokenGenerator(IConfiguration configuration, IOptions<SecurityConfig> securitySettions)
    {
        _configuration = configuration;
        _securitySettions = securitySettions.Value;
    }

    public string GenerateToken(IUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securitySettions?.IssuerKey ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
           new Claim(ClaimTypes.NameIdentifier, user.Id),
           new Claim(ClaimTypes.Name, user.Username),
           new Claim(ClaimTypes.Role, user.Role)
       };

        var dtExpire = DateTime.Now.AddMinutes(_securitySettions?.TokenExp ?? 10);
        var tokenSecurity = new JwtSecurityToken(
            issuer: _securitySettions?.Issuer,
            audience: _securitySettions?.TokenAudirence,
            claims: claims,
            expires: dtExpire,
            signingCredentials: creds
        );

        var token = tokenHandler.WriteToken(tokenSecurity);
        return token;
    }
}
