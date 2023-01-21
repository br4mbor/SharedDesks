using Abb.Euopc.SharedDesks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abb.Euopc.SharedDesks.EF.EntityConfigurations;

internal sealed class DeskItemToDeskEntityConfiguration : IEntityTypeConfiguration<DeskItemToDesk>
{
    public void Configure(EntityTypeBuilder<DeskItemToDesk> builder)
    {
        builder.HasOne(ditd => ditd.Desk)
            .WithMany(d => d.DeskItemsToDesks)
            .HasForeignKey(ditd => ditd.DeskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ditd => ditd.DeskItem)
            .WithMany()
            .HasForeignKey(ditd => ditd.DeskItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
