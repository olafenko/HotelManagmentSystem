using HotelManageSys.API.Features.Auth.Messages.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelManageSys.API.Controllers;



[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
    {

        var token = await _mediator.Send(loginCommand);

        return Ok(new {Token = token});

    }
    
    
}