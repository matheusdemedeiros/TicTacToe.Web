using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.UseCases.Match.AddPlayer;
using TicTacToe.Application.UseCases.Match.CreateMatch;

namespace TicTacToe.Infra.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new TicTacToe match.
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateTicMatchResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMatch(CreateTicMatchCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.MatchId != Guid.Empty)
            {
                return Created(result.MatchId.ToString(), result);
            }

            return BadRequest("Failed to create match.");
        }

        /// <summary>
        /// Add a player to an existing TicTacToe match.
        /// </summary>
        /// <param name="matchId">Id from match to add player.</param>
        /// <param name="command">Command</param>
        /// <returns></returns>
        [HttpPost("/{matchId}/add-player")]
        [ProducesResponseType(typeof(AddTicPlayerPlayerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPlayer(Guid matchId, AddTicPlayerToMatchCommand command)
        {
            command.MatchId = matchId.ToString();
            var result = await _mediator.Send(command);

            if (result.MatchId != Guid.Empty)
            {
                return Ok(result);
            }

            return BadRequest("Failed to add player.");
        }
    }
}
