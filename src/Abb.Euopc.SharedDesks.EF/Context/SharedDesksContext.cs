using Abb.Euopc.SharedDesks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Abb.Euopc.SharedDesks.EF.Context;

internal sealed class SharedDesksContext : DbContext
{
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Desk> Desks => Set<Desk>();
    public DbSet<DeskItem> DeskItems => Set<DeskItem>();
    public DbSet<DeskItemType> DeskItemTypes => Set<DeskItemType>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    public SharedDesksContext(DbContextOptions<SharedDesksContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SharedDesksContext).Assembly);
    }
}
