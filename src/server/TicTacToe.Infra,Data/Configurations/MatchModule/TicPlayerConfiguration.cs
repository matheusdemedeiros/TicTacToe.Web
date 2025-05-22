using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Infra_Data.Configurations.MatchModule
{
    public class TicPlayerConfiguration : IEntityTypeConfiguration<TicPlayer>
    {
        public void Configure(EntityTypeBuilder<TicPlayer> builder)
        {
            builder.ToTable("TicPlayers");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.NickName)
                .IsRequired()
                 .HasMaxLength(150);

            builder.Property(p => p.Symbol)
                .IsRequired()
                .HasMaxLength(1);
        }
    }
}
