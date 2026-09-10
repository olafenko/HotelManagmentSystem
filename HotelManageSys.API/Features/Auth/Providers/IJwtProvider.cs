using HotelManageSys.API.Models;

namespace HotelManageSys.API.Features.Auth.Services;

public interface IJwtProvider
{
    string GenerateToken(Worker worker);
}