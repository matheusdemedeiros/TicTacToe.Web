using MediatR;

namespace TicTacToe.Application.UseCases.Match.CreateMatch
{
    public class CreateMatchHandler : IRequestHandler<CreateMatchCommand, CreateMatchResponse>
    {
        public Task<CreateMatchResponse> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
        {

        }
    }
}
