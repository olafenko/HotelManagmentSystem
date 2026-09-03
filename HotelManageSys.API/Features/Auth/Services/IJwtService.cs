using HotelManageSys.API.Models;

namespace HotelManageSys.API.Features.Auth.Services;

public interface IJwtService
{
    string GenerateToken(Worker worker);
}