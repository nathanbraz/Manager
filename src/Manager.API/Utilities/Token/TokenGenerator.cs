using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Manager.API.Utilities.Token
{
  public class TokenGenerator : ITokenGenerator
  {

    private readonly IConfiguration _configuration;

    public TokenGenerator(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string GenerateToken(string login)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity(new Claim[]
          {
              new Claim(ClaimTypes.Name, login),
              new Claim(ClaimTypes.Role, "User")
          }),
          Expires = DateTime.UtcNow.AddMinutes(1),
          SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
  }
}