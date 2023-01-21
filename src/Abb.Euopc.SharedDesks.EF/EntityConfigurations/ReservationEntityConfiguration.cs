using Abb.Euopc.SharedDesks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abb.Euopc.SharedDesks.EF.EntityConfigurations;

internal sealed class ReservationEntityConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasOne(r => r.Desk)
            .WithMany()
            .HasForeignKey(r => r.DeskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(r => r.CreatedByEmail)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.ReservedForEmail)
            .IsRequired()
            .HasMaxLength(100);
    }
}
