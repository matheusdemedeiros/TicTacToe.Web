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

        /// <summary>
        /// Create a new TicTacToe player.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateTicPlayerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePlayerAsync(CreateTicPlayerCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.id != null)
            {
                return Created(result.id.ToString(), result);   
            }

            return BadRequest("Failed to create player.");
        }
    }
}
