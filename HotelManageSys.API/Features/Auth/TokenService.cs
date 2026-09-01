using System.Security.Claims;
using HotelManageSys.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace HotelManageSys.API.Auth;

public class TokenService
{

    private readonly IConfiguration _config;
    private readonly string _secretKey;
    private readonly int _accessTokenExpirationMinutes;

    public TokenService(IConfiguration config, string secretKey, int accessTokenExpirationMinutes)
    {
        _config = config;
        _secretKey = secretKey;
        _accessTokenExpirationMinutes = accessTokenExpirationMinutes;
    }

    public string GenerateToken(Worker worker)
    {

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, worker.WorkerId.ToString()),
            new Claim(ClaimTypes.Name, worker.FirstName),
            new Claim(ClaimTypes.Name, worker.LastName),
            new Claim(ClaimTypes.Role, worker.Role.ToString())
        };


        //TODO zaimpplementowac dalsza czesc

        return "";
    }
    
    
    
}