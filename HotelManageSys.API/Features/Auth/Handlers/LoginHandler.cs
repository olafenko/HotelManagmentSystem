using HotelManageSys.API.Exceptions;
using HotelManageSys.API.Features.Auth.Messages.Commands;
using HotelManageSys.API.Features.Auth.Services;
using HotelManageSys.API.Features.Workers.Providers;
using HotelManageSys.API.Models.Data;
using MediatR;

namespace HotelManageSys.API.Features.Auth.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, string>
{

    private readonly IJwtProvider _jwtProvider;
    private readonly IWorkerProvider _workerProvider;
    private readonly ApplicationDbContext _context;

    public LoginHandler(IJwtProvider jwtProvider, IWorkerProvider workerProvider, ApplicationDbContext context)
    {
        _jwtProvider = jwtProvider;
        _workerProvider = workerProvider;
        _context = context;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var worker = await _workerProvider.GetWorkerByLoginAsync(request.Login,false,cancellationToken);

        if (worker == null || worker.Password != request.Password)
        {
            throw new InvalidCredentialsException();
        }

        return _jwtProvider.GenerateToken(worker);
    }
}