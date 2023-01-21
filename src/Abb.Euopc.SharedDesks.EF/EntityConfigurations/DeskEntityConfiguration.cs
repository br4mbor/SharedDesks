using Abb.Euopc.SharedDesks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abb.Euopc.SharedDesks.EF.EntityConfigurations;

internal sealed class DeskEntityConfiguration : IEntityTypeConfiguration<Desk>
{
    public void Configure(EntityTypeBuilder<Desk> builder)
    {
        builder.HasOne(d => d.Area)
            .WithMany()
            .HasForeignKey(d => d.AreaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(d => d.Label)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(d => d.ImageUrl)
            .HasMaxLength(256);

        builder.Ignore(d => d.ItemsCount);
        builder.Ignore(d => d.ReservationsCount);
        builder.Ignore(d => d.Reservation);
    }
}
