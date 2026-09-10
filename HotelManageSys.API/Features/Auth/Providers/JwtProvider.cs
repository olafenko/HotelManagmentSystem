using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelManageSys.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace HotelManageSys.API.Features.Auth.Services;

public class JwtProvider : IJwtProvider
{

    private readonly IConfiguration _config;
    private readonly string _secretKey;
    private readonly int _accessTokenExpirationMinutes;

    public JwtProvider(IConfiguration config)
    {
        _config = config;
        _secretKey = config.GetValue<string>("JwtConfig:SecretKey");
        _accessTokenExpirationMinutes = config.GetValue("JwtConfig:AccessTokenExpirationMinutes", 15);
    }

    public string GenerateToken(Worker worker)
    {

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, worker.WorkerId.ToString()),
            new Claim(ClaimTypes.Name, $"{worker.FirstName} {worker.LastName}"),
            new Claim(ClaimTypes.Role, worker.Role.ToString())
        };


        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

        var signingCreds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            
            issuer: _config["JwtConfig:Issuer"],
            audience: _config["JwtConfig:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(_accessTokenExpirationMinutes),
            signingCredentials: signingCreds
            );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    
    
}