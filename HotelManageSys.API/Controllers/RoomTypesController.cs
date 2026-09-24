using HotelManageSys.API.Features.RoomTypes.DTO_s;
using HotelManageSys.API.Features.RoomTypes.Messages.Commands;
using HotelManageSys.API.Features.RoomTypes.Messages.Queries;
using HotelManageSys.API.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManageSys.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = nameof(Role.ADMIN) + "," + nameof(Role.MANAGER) + "," + nameof(Role.WORKER))]
        [HttpGet]
        [ProducesResponseType(typeof(List<RoomTypeDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRoomTypes()
        {
            var query = new GetAllRoomTypesQuery();
            return Ok(await _mediator.Send(query));
        }

        [Authorize(Roles = nameof(Role.ADMIN) + "," + nameof(Role.MANAGER) + "," + nameof(Role.WORKER))]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoomTypeDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoomTypeById(int id)
        {
            var query = new GetRoomTypeByIdQuery(id);


            return Ok(await _mediator.Send(query));
        }

        [Authorize(Roles = nameof(Role.ADMIN) + "," + nameof(Role.MANAGER))]
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRoomType([FromBody] CreateRoomTypeCommand createCommand)
        {
            var roomTypeId = await _mediator.Send(createCommand);

            return CreatedAtAction(
                nameof(GetRoomTypeById),
                new { id = roomTypeId },
                new { id = roomTypeId, message = "Typ pokoju został utworzony" }
            );
        }

        [Authorize(Roles = nameof(Role.ADMIN) + "," + nameof(Role.MANAGER) + "," + nameof(Role.WORKER))]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoomType(int id, [FromBody] UpdateRoomTypeCommand updateCommand)
        {
            if (id != updateCommand.RoomTypeId)
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
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            var deleteCommand = new DeleteRoomTypeCommand(id);

            await _mediator.Send(deleteCommand);
            return NoContent();
        }
    }
}