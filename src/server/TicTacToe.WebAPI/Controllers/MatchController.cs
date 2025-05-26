using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> CreateMatch(CreateTicMatchCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.MatchId != Guid.Empty)
            {
                return Created(result.MatchId.ToString(), result);
            }

            return BadRequest("Failed to create match.");
        }
    }
}
