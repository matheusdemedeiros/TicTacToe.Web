using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Infra_Data.Configurations.MatchModule
{
    public class TicMatchConfiguration : IEntityTypeConfiguration<TicMatch>
    {
        public void Configure(EntityTypeBuilder<TicMatch> builder)
        {
            builder.ToTable("TicMatches");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.State)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(p => p.PlayMode)
                .IsRequired()
                .HasColumnType("int");

            builder.HasOne(p => p.TicScore)
                .WithOne()
                .HasForeignKey<TicScore>(b => b.MatchId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.OwnsOne(p => p.Board, board =>
            {
                board.ToJson();

                board.Property(b => b.WinningSimbol)
                     .HasColumnName("WinningSimbol")
                     .HasMaxLength(1);
            });

            builder.HasMany(m => m.Players)
                .WithMany(p => p.Matches)
                .UsingEntity<Dictionary<string, object>>(
                    "TicMatchPlayers",
                    right => right.HasOne<TicPlayer>()
                                  .WithMany()
                                  .HasForeignKey("PlayerId")
                                  .OnDelete(DeleteBehavior.Cascade),
                    left => left.HasOne<TicMatch>()
                                .WithMany()
                                .HasForeignKey("MatchId")
                                .OnDelete(DeleteBehavior.Cascade),
                    join =>
                    {
                        join.HasKey("MatchId", "PlayerId");
                        join.ToTable("TicMatchPlayers");
                        join.Property<Guid>("MatchId");
                        join.Property<Guid>("PlayerId");
                    });
        }
    }
}
