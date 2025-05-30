using TicTacToe.Domain.Entities.BaseModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Domain.Entities.MatchModule
{
    public class TicPlayer : BaseEntity
    {
        public string Name { get; set; }
        public string NickName { get; set; }
        public string? Symbol { get; set; }

        public virtual List<TicMatch> Matches { get; set; }

        public TicPlayer(string name, string nickName) : base()
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(nickName))
            {
                throw new DomainException("Name and NickName cannot be null or empty.");
            }

            //if (string.IsNullOrEmpty(symbol))
            //{
            //    throw new DomainException("TicPlayer symbol is required.");
            //}

            Name = name;
            NickName = nickName;
        }

        public void SetSymbol(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                throw new DomainException("TicPlayer symbol cannot be null or empty.");
            }

            Symbol = symbol;
        }
    }
}
