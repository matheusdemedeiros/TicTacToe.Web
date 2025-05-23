using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.UseCases.Match.CreatePlayer;

namespace TicTacToe.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlayerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayerAsync(CreateTicPlayerCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.id != null)
            {
                return Created();
            }

            return BadRequest("Failed to create player.");
        }
    }
}
