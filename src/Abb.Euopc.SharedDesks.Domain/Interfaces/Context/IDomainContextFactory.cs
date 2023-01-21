namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Context;

public interface IDomainContextFactory
{
    IDomainContext CreateDomainContext();
}
