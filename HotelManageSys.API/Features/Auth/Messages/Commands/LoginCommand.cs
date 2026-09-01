using MediatR;

namespace HotelManageSys.API.Auth.DTO_s;

public class LoginCommand : IRequest<bool>
{
    public required string Login { get; set; }
    public required string Password { get; set; }

}