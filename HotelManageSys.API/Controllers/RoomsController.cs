using HotelManageSys.API.Features.Rooms.Messages.Commands;
using HotelManageSys.API.Features.Rooms.Messages.Queries;
using HotelManageSys.API.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManageSys.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = nameof(Role.ADMIN) + "," + nameof(Role.MANAGER) + "," + nameof(Role.WORKER))]
        [HttpGet]
        [ProducesResponseType(typeof(List<RoomDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRooms()
        {
            var query = new GetAllRoomsQuery();

            return Ok(await _mediator.Send(query));
        }

        [Authorize(Roles = nameof(Role.ADMIN) + "," + nameof(Role.MANAGER) + "," + nameof(Role.WORKER))]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoomDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var query = new GetRoomByIdQuery(id);

            return Ok(await _mediator.Send(query));
        }

        [Authorize(Roles = nameof(Role.ADMIN) + "," + nameof(Role.MANAGER))]
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand createCommand)
        {
            int roomId;

            roomId = await _mediator.Send(createCommand);

            return CreatedAtAction(
                nameof(GetRoomById),
                new { id = roomId },
                new { id = roomId, message = "Pokój został utworzony" }
            );
        }

        [Authorize(Roles = nameof(Role.ADMIN) + "," + nameof(Role.MANAGER) + "," + nameof(Role.WORKER))]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomCommand updateCommand)
        {
            if (id != updateCommand.RoomId)
            {
                return BadRequest("Id w URL nie jest takie samo jak w body");
            }

            await _mediator.Send(updateCommand);
            return NoContent();
        }

        [Authorize(Roles = nameof(Role.ADMIN) + "," + nameof(Role.MANAGER))]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var deleteCommand = new DeleteRoomCommand(id);

            await _mediator.Send(deleteCommand);
            return NoContent();
        }
    }
}