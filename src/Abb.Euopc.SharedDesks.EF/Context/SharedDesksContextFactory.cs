using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Abb.Euopc.SharedDesks.EF.Context;

internal sealed class SharedDesksContextFactory : IDesignTimeDbContextFactory<SharedDesksContext>
{
    public SharedDesksContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SharedDesksContext>();

        optionsBuilder.UseSqlServer("***");
        return new SharedDesksContext(optionsBuilder.Options);
    }
}

