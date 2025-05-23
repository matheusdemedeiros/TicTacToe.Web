using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Infra.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateMatch(int id)
        {
        }
    }
}
