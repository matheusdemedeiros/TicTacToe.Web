using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Domain.MatchModule
{
    public class TicPlayer
    {
        public string Name { get; private set; }
        public string NickName { get; private set; }
        public string Symbol { get; private set; }

        public TicPlayer(string name, string nickName)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(nickName))
            {
                throw new DomainException("Name and NickName cannot be null or empty.");
            }

            Name = name;
            NickName = nickName;
        }
    }
}
