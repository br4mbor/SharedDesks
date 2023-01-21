using Abb.Euopc.SharedDesks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abb.Euopc.SharedDesks.EF.EntityConfigurations;

internal sealed class DeskItemTypeEntityConfiguration : IEntityTypeConfiguration<DeskItemType>
{
    public void Configure(EntityTypeBuilder<DeskItemType> builder)
    {
        builder.HasMany<DeskItem>()
            .WithOne(di => di.Type)
            .HasForeignKey(di => di.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(dit => dit.Name)
            .IsRequired()
            .HasMaxLength(30);
    }
}
