using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Infra_Data.Configurations.MatchModule
{
    public class TicScoreConfiguration : IEntityTypeConfiguration<TicScore>
    {
        public void Configure(EntityTypeBuilder<TicScore> builder)
        {
            builder.ToTable("TicScores");
            
            builder.HasKey(p => p.Id);

            builder.Property(x => x.WinningSymbol)
                .IsRequired(false)
                .HasMaxLength(1);

            builder.Property(x => x.Tie)
                    .IsRequired();

            builder.HasOne(x => x.Match)
                .WithOne(x => x.TicScore)
                .HasForeignKey<TicScore>(x => x.MatchId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
