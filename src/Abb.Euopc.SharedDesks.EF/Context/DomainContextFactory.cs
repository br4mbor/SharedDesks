using Abb.Euopc.SharedDesks.Domain.Interfaces.Context;
using Microsoft.EntityFrameworkCore;

namespace Abb.Euopc.SharedDesks.EF.Context;

internal sealed class DomainContextFactory : IDomainContextFactory
{
    private readonly IDbContextFactory<SharedDesksContext> _dbContextFactory;


    public DomainContextFactory(IDbContextFactory<SharedDesksContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public IDomainContext CreateDomainContext()
        => new DomainContext(_dbContextFactory.CreateDbContext());
}
