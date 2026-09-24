using MediatR;

namespace HotelManageSys.API.Features.Auth.Messages.Commands;

public class LoginCommand : IRequest<string>
{
    public required string Login { get; set; }
    public required string Password { get; set; }

}