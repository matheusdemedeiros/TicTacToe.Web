using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Infra_Data.Configurations.MatchModule
{
    public class TicBoardConfiguration : IEntityTypeConfiguration<TicBoard>
    {
        public void Configure(EntityTypeBuilder<TicBoard> builder)
        {
            builder.ToTable("TicBoards");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.WinningSimbol)
                   .HasMaxLength(1);

            builder.Property(x => x.SerializedBoard)
                   .HasColumnName("Board")
                   .HasColumnType("nvarchar(max)");
        }
    }
}
