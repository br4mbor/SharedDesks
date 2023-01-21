using Abb.Euopc.SharedDesks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abb.Euopc.SharedDesks.EF.EntityConfigurations;

internal sealed class DeskItemEntityConfiguration : IEntityTypeConfiguration<DeskItem>
{
    public void Configure(EntityTypeBuilder<DeskItem> builder)
    {
        builder.Property(di => di.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
